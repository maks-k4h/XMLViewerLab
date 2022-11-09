namespace XMLViewer;

using Controllers;

public partial class MainPage : ContentPage
{
    private XMLViewerController _controller;

    private ScrollView resultScrollView;
    
    public MainPage()
    {
        InitializeComponent();

        _controller = new XMLViewerController();

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
                "LINQ",
                "SAX",
                "DOM"
            },
            SelectedIndex = 0,
            FontSize = 18,
            TextColor = Colors.Black,
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
        resultScrollView.Content = new VerticalStackLayout
        {
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
            new Label{Text = _controller.TestResult, FontSize = 50},
        };
        
    }

    private Grid GetTextFilerElement(string filterName, XMLViewerController.TextSetter textSetter, XMLViewerController.FilterUseSetter filterUseSetter)
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
    
    private HorizontalStackLayout GetDateFilerElement(string filterName, XMLViewerController.DateSetter dateSetter, XMLViewerController.FilterUseSetter filterUseSetter)
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