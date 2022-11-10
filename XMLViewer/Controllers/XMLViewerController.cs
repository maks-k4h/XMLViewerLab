using XMLViewer.lib;
using XMLViewer.Models;

namespace XMLViewer.Controllers;

public class XmlViewerController
{
    public List<Article> Result;
    
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

    private AnalyzerContext _analyzerContext;

    public XmlViewerController()
    {
        _analyzerContext = new AnalyzerContext();
        _analyzerContext.SetStrategy(AnalyzerContext.XmlAnalysisStrategy.Dom);
        _analyzerContext.SetFilePath("../Data/file1.xml");
    }

    public void SetTitle(string title)
    {
        Run();
    }

    public void SetAuthor(string author)
    {
        Run();
    }

    public void SetCategory(string category)
    {
        Run();
    }

    public void SetFromDate(DateTime formDate)
    {
        Run();
    }

    public void SetToDate(DateTime toDate)
    {
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

    public void SetMethod(AnalyzerContext.XmlAnalysisStrategy strategy)
    {
        try
        {
            _analyzerContext.SetStrategy(strategy);
        }
        catch
        {
            // ignored
        }
        Run();
    }

    private void Run()
    {
        try
        {
            Result = _analyzerContext.Run();
        }
        catch
        {
            // ignored
        }
        
        if (ResultUpdated != null) ResultUpdated();
    }

}