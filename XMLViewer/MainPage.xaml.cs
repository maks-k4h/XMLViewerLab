using XMLViewer.lib;
using XMLViewer.Models;

namespace XMLViewer;

using Controllers;

public partial class MainPage : ContentPage
{
    private XmlViewerController _controller;
    private ScrollView _resultScrollView;
    
    public MainPage()
    {
        InitializeComponent();
        _controller = new XmlViewerController();
        RenderPage();
        _controller.ResultUpdated += RenderResult;  // called on every result update
    }
    
    private void RenderPage()
    {
        var mainGrid = new Grid // backbone grid of the page
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(70) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
            }
        };

        var parseMethodSelector = new Picker
        {
            Items =
            {
                ((AnalyzerContext.XmlAnalysisStrategy)0).ToString(),
                ((AnalyzerContext.XmlAnalysisStrategy)1).ToString(),
                ((AnalyzerContext.XmlAnalysisStrategy)2).ToString(),
            },
            Title = "select",
            FontSize = 20,
            Margin = new Thickness(0, 3, 0, 0),
            TextColor = Colors.Black,
        };

        parseMethodSelector.SelectedIndexChanged += (sender, args) =>
        { _controller.SetMethod((AnalyzerContext.XmlAnalysisStrategy)parseMethodSelector.SelectedIndex); };

        _resultScrollView = new ScrollView();
        
        mainGrid.SetRowSpan(_resultScrollView, 2);
        mainGrid.Add(_resultScrollView, 1);
        mainGrid.Add(new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 20,
            Children =
            {
                new Label{Text = "Filters", FontSize = 30},
                GetTextFilterElement("Title",    _controller.SetTitleFilter,    _controller.SetTitleFilterUse),
                GetTextFilterElement("Author",   _controller.SetAuthorFilter,   _controller.SetAuthorFilterUse),
                GetTextFilterElement("Category", _controller.SetCategoryFilter, _controller.SetCategoryFilterUse),
                GetDateFilterElement("From",     _controller.SetFromDateFilter, _controller.SetFromDateFilterUse,   new DateTime(2000, 1, 1)),
                GetDateFilterElement("To",       _controller.SetToDateFilter,   _controller.SetToDateFilterUse,     DateTime.Now),
            }
        }, 0);
        mainGrid.Add(new HorizontalStackLayout
        { 
            Margin = 20, 
            Children = 
            {
                new Label{Text = "Method: ", FontSize = 23},
                parseMethodSelector,
            }
        }, 0, 1);
        
        Content = mainGrid;
    }

    private void RenderResult()
    {
        var stackLayout = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = 15
        };
        if (_controller.Result != null)
            foreach (var article in _controller.Result)
            {
                stackLayout.Add(GetArticleRepresentation(article));
            }

        _resultScrollView.Content = stackLayout;
    }

    private IView GetArticleRepresentation(Article article)
    {
        var stackView = new VerticalStackLayout { Spacing  = 5 };
        
        // Title
        stackView.Add(new Label { Text = article.Title, FontSize = 28 });
        
        // Visual line
        stackView.Add(new BoxView { Color = Color.FromHex("#f7ae48"), HeightRequest = 3 });
        
        // Annotation
        stackView.Add(new Label { Text = article.Annotation, FontSize = 19 });
        
        // Category
        stackView.Add(new Label { Text = "Category: " + article.Category, FontSize = 19, });
        
        // Date
        if (article.Date != null)
            stackView.Add(new Label { Text = "Date: " + article.Date.Value.ToShortDateString(), FontSize = 19, });

        // Author
        stackView.Add(new Label { Text = "By " + article.Author, FontSize = 19, });
        
        // Stack of reviews
        var reviewsView = new VerticalStackLayout { Padding = new Thickness(5,0) };
        
        foreach (var review in article.Reviews)
        {
            // every review
            reviewsView.Add(new HorizontalStackLayout
            {
                HeightRequest = 30,
                Spacing = 5,
                Children =
                {
                    new Image
                    { 
                        Source = "quote.png",
                        HeightRequest = 15,
                        WidthRequest = 17,
                        HorizontalOptions = LayoutOptions.Start
                    },
                    new Label
                    {
                        Text = review.Text, 
                        FontSize = 19, 
                        VerticalOptions = LayoutOptions.End
                    },
                }
            });
        }
        
        stackView.Add(reviewsView);
        return new Frame { Content = stackView };
    }

    private Grid GetTextFilterElement(string filterName, XmlViewerController.TextSetter textSetter, XmlViewerController.FilterUseSetter filterUseSetter)
    {
        var filterGrid = new Grid
        {
            RowSpacing = 5,
            RowDefinitions =
            {
                new RowDefinition{ Height = 30 },
                new RowDefinition{ Height = 40 },
            },
            ColumnDefinitions =
            {
                new ColumnDefinition{Width = new GridLength(50)},
                new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)}
            }
        };

        var checkBox = new CheckBox();
        var label = new Label { FontSize = 22, Text = filterName };
        var entry = new Entry { BackgroundColor = Colors.WhiteSmoke, FontSize = 20 };
        
        entry.Completed += async (sender, args) => { textSetter(entry.Text); };
        checkBox.CheckedChanged += async (sender, e) => { filterUseSetter(e.Value); };
        
        filterGrid.Add(checkBox, 0);
        filterGrid.Add(label, 1);
        filterGrid.Add(entry, 1, 1);

        return filterGrid;
    }
    
    private HorizontalStackLayout GetDateFilterElement(string filterName, XmlViewerController.DateSetter dateSetter, XmlViewerController.FilterUseSetter filterUseSetter, DateTime date)
    {
        var filterStack = new HorizontalStackLayout()
        {
            HeightRequest = 30,
            Margin = new Thickness(5,0,0,0)
        };

        var label = new Label { FontSize = 22, Text = filterName };
        var checkBox = new CheckBox();
        var picker = new DatePicker
        {
            Date = date,
            FontFamily = "OpenSansRegular", 
            Margin = new Thickness(10, 7, 0, 0),
            TextColor = Colors.Black
        };
        
        picker.DateSelected += (sender, args) => { dateSetter(picker.Date); };
        checkBox.CheckedChanged += (sender, e) => { filterUseSetter(e.Value); };
        
        filterStack.Add(checkBox);
        filterStack.Add(label);
        filterStack.Add(picker);

        return filterStack;
    }

    private async void ExportClicked(object sender, EventArgs e)
    {
        var path = _controller.ExportHtml();
        await Launcher.Default.OpenAsync("file://" + path);
    }

    private async void GetHelpClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HelpPage));
    }
}