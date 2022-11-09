namespace XMLViewer.Controllers;

public class XMLViewerController
{
    public string TestResult;
    
    public delegate void TextSetter(string s);
    public delegate void DateSetter(DateTime d);
    public delegate void FilterUseSetter(bool b);

    public delegate void ResultUpdatedHandler();
    public event ResultUpdatedHandler ResultUpdated;

    private bool _useTitleFilter        = false;
    private bool _useAuthorFilter       = false;
    private bool _useCategoryFilter     = false;
    private bool _useFromDateFilter     = false;
    private bool _useToDateFilter       = false;

    public void SetTitle(string title)
    {
        TestResult = "Title";
        Run();
    }

    public void SetAuthor(string author)
    {
        TestResult = "Author";
        Run();
    }

    public void SetCategory(string category)
    {
        TestResult = "Category";
        Run();
    }

    public void SetFromDate(DateTime formDate)
    {
        TestResult = "FromDate";
        Run();
    }

    public void SetToDate(DateTime toDate)
    {
        TestResult = "ToDate";
        Run();
    }
    
    public void SetTitleFilterUse(bool b)
    {
        _useTitleFilter = b;
        Run();
    }
    
    public void SetAuthorFilterUse(bool b)
    {
        _useAuthorFilter = b;
        Run();
    }
    
    public void SetCategoryFilterUse(bool b)
    {
        _useCategoryFilter = b;
        Run();
    }
    
    public void SetFromDateFilterUse(bool b)
    {
        _useFromDateFilter = b;
        Run();
    }
    
    public void SetToDateFilterUse(bool b)
    {
        _useToDateFilter = b;
        Run();
    }

    private void Run()
    {
        if (_useAuthorFilter)
            TestResult += "hahaha";
        if (ResultUpdated != null) ResultUpdated();
    }

}