namespace XMLViewer;

public partial class OpenFilePage : ContentPage
{
    public OpenFilePage()
    {
        InitializeComponent();
        RenderPage();
    }

    private void RenderPage()
    {
        var filePathEntry = new Entry
        {
            FontSize = 20,
            Placeholder = "XML file path...",
            HeightRequest = 38,
            WidthRequest = 300,
        };
        var button = new Button
        {
            FontSize = 20,
            Text = "Open",
            HeightRequest = 38,
            WidthRequest = 90,
            Margin = new Thickness(5, 0, 0, 0)
        };
        Content = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions =LayoutOptions.Center,
            Children =
            { new HorizontalStackLayout { filePathEntry, button } }
        };
        
        // adding a handler
        button.Clicked += (sender, args) => OpenClicked(filePathEntry.Text ?? " "); 
    }

    private void OpenClicked(string s)
    {
        Data.FileInfo.FilePath = s;
        Shell.Current.GoToAsync("..");
    }
}