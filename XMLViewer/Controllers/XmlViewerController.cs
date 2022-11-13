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

    private AnalyzerContext _analyzerContext;

    public XmlViewerController()
    {
        _analyzerContext = new AnalyzerContext();
    }

    public void SetTitleFilter(string title)
    {
        _analyzerContext.SetTitleFilter(title);
        Run();
    }

    public void SetAuthorFilter(string author)
    {
        _analyzerContext.SetAuthorFilter(author);
        Run();
    }

    public void SetCategoryFilter(string category)
    {
        _analyzerContext.SetCategoryFilter(category);
        Run();
    }

    public void SetFromDateFilter(DateTime formDate)
    {
        _analyzerContext.SetFromDateFilter(formDate);
        Run();
    }

    public void SetToDateFilter(DateTime toDate)
    {
        _analyzerContext.SetToDateFilter(toDate);
        Run();
    }
    
    public void SetTitleFilterUse(bool b)
    {
        _analyzerContext.SetTitleFilterUse(b);
        Run();
    }
    
    public void SetAuthorFilterUse(bool b)
    {
        _analyzerContext.SetAuthorFilterUse(b);
        Run();
    }
    
    public void SetCategoryFilterUse(bool b)
    {
        _analyzerContext.SetCategoryFilterUse(b);
        Run();
    }
    
    public void SetFromDateFilterUse(bool b)
    {
        _analyzerContext.SetFromDateFilterUse(b);
        Run();
    }
    
    public void SetToDateFilterUse(bool b)
    {
        _analyzerContext.SetToDateFilterUse(b);
        Run();
    }

    public void SetMethod(AnalyzerContext.XmlAnalysisStrategy strategy)
    {
        try
        {
            _analyzerContext.SetStrategy(strategy);
            _analyzerContext.SetFilePath("../Data/file1.xml");
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

        ResultUpdated?.Invoke();
    }

}