namespace XMLViewer.lib;

public struct ArticleFilter
{
    public bool UseTitleFilter      { get; set; }
    public bool UseAuthorFilter     { get; set; }
    public bool UseCategoryFilter   { get; set; }
    public bool UseFromDateFilter   { get; set; }
    public bool UseToDateFilter     { get; set; }
    
    public string TitleFilter
    {
        get => _title;
        set => _title = value.ToLower().Trim();
    }
    public string CategoryFilter
    {
        get => _category;
        set => _category = value.ToLower().Trim();
    }
    public string AuthorFilter
    {
        get => _author;
        set => _author = value.ToLower().Trim();
    }

    public DateTime? ToDateFilter   { get; set; } = null;
    public DateTime? FromDateFilter { get; set; } = null;

    private string _title       = "";
    private string _category    = "";
    private string _author      = "";

    public ArticleFilter()
    {
    }
}