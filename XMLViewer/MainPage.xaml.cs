using XMLViewer.lib;
using XMLViewer.Models;

namespace XMLViewer;

using Controllers;

public partial class MainPage : ContentPage
{
    private XmlViewerController _controller;

    private ScrollView resultScrollView;
    
    public MainPage()
    {
        InitializeComponent();

        _controller = new XmlViewerController();

        RenderPage();

        _controller.ResultUpdated += RenderResult;
    }
    
    private void RenderPage()
    {
        var mainGrid = new Grid
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
                ((lib.AnalyzerContext.XmlAnalysisStrategy)0).ToString(),
                ((lib.AnalyzerContext.XmlAnalysisStrategy)1).ToString(),
                ((lib.AnalyzerContext.XmlAnalysisStrategy)2).ToString(),
            },
            Title = "select",
            FontSize = 18,
            TextColor = Colors.Black,
        };

        parseMethodSelector.SelectedIndexChanged += (sender, args) =>
        {
            _controller.SetMethod((AnalyzerContext.XmlAnalysisStrategy)parseMethodSelector.SelectedIndex);
        };

        resultScrollView = new ScrollView();
        
        mainGrid.SetRowSpan(resultScrollView, 2);
        mainGrid.Add(resultScrollView, 1);
        mainGrid.Add(new HorizontalStackLayout
        { 
            Margin = 20, 
            Children = 
            {
               new Label{Text = "Method: ", FontSize = 23},
               parseMethodSelector,
            }
        }, 0, 1);
        mainGrid.Add(new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 20,
            Children =
            {
                new Label{Text = "Filters", FontSize = 30},
                GetTextFilerElement("Title",    _controller.SetTitle,       _controller.SetTitleFilterUse),
                GetTextFilerElement("Author",   _controller.SetAuthor,      _controller.SetAuthorFilterUse),
                GetTextFilerElement("Category", _controller.SetCategory,    _controller.SetCategoryFilterUse),
                GetDateFilerElement("From",     _controller.SetFromDate,    _controller.SetFromDateFilterUse),
                GetDateFilerElement("To",       _controller.SetToDate,      _controller.SetToDateFilterUse),
            }
        }, 0);
        

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

        resultScrollView.Content = stackLayout;
    }

    private IView GetArticleRepresentation(Article article)
    {
        var stackView = new VerticalStackLayout
        {
            Spacing  = 5,
        };
        
        // Title
        stackView.Add(new Label
        {
            Text = article.Title,
            FontSize = 28
        });
        
        // Visual line
        stackView.Add(new BoxView{Color = Color.FromHex("#f7ae48"), HeightRequest = 3});
        
        // Annotation
        stackView.Add(new Label
        {
            Text = article.Annotation,
            FontSize = 19,
        });
        
        // Category
        stackView.Add(new Label
        {
            Text = "Category: " + article.Category,
            FontSize = 19,
        });
        
        // Author
        stackView.Add(new Label
        {
            Text = "By " + article.Author,
            FontSize = 19,
        });
        
        // Stack of reviews
        var reviewsView = new VerticalStackLayout
        {
            Padding = new Thickness(5,0)
        };
        
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
                    new Label { Text = review.Text, FontSize = 19, VerticalOptions = LayoutOptions.End},
                }
            });
        }
        
        stackView.Add(reviewsView);
        
        return new Frame
        {
            Content = stackView
        };
    }

    private Grid GetTextFilerElement(string filterName, XmlViewerController.TextSetter textSetter, XmlViewerController.FilterUseSetter filterUseSetter)
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
        
        entry.Completed += (sender, args) => { textSetter(entry.Text); };
        checkBox.CheckedChanged += (sender, e) => { filterUseSetter(e.Value); };
        
        filterGrid.Add(checkBox, 0);
        filterGrid.Add(label, 1);
        filterGrid.Add(entry, 1, 1);

        return filterGrid;
    }
    
    private HorizontalStackLayout GetDateFilerElement(string filterName, XmlViewerController.DateSetter dateSetter, XmlViewerController.FilterUseSetter filterUseSetter)
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
            FontSize = 20, 
            FontFamily = "OpenSansRegular", 
            Margin = new Thickness(10, 8, 0, 0)
        };
        
        picker.DateSelected += (sender, args) => { dateSetter(picker.Date); };
        checkBox.CheckedChanged += (sender, e) => { filterUseSetter(e.Value); };
        
        filterStack.Add(checkBox);
        filterStack.Add(label);
        filterStack.Add(picker);

        return filterStack;
    }
}