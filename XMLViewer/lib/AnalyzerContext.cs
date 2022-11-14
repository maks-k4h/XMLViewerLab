using XMLTest.lib;
using XMLViewer.Models;

namespace XMLViewer.lib;

// The context for simple interaction with strategies
public class AnalyzerContext
{
    private XmlAnalyzerStrategy _analyzer;
    private ArticleFilter _filter;
    
    public enum XmlAnalysisStrategy
    {
        Dom,
        Linq,
        Sax
    }

    public AnalyzerContext()
    {
        _filter = new ArticleFilter();
    }

    public void SetStrategy(XmlAnalysisStrategy s)
    {
        switch (s)
        {
            case XmlAnalysisStrategy.Dom:
                _analyzer = new DomAnalyzer();
                break;
            case XmlAnalysisStrategy.Linq:
                throw new NotImplementedException();
            case XmlAnalysisStrategy.Sax:
                _analyzer = new SaxAnalyzer();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void SetFilePath(string path)
    {
        _analyzer?.SetFilePath(path);
    }

    public List<Article> Run()
    {
        return _analyzer.Analyze(_filter);
    }
    
    public void SetTitleFilter(string s)
    {
        _filter.TitleFilter = s;
    }

    public void SetTitleFilterUse(bool b)
    {
        _filter.UseTitleFilter = b;
    }
    
    public void SetAuthorFilter(string s)
    {
        _filter.AuthorFilter = s;
    }

    public void SetAuthorFilterUse(bool b)
    {
        _filter.UseAuthorFilter = b;
    }

    public void SetCategoryFilter(string s)
    {
        _filter.CategoryFilter = s;
    }
    
    public void SetCategoryFilterUse(bool b)
    {
        _filter.UseCategoryFilter = b;
    }

    public void SetFromDateFilter(DateTime? date)
    {
        _filter.FromDateFilter = date;
    }
    
    public void SetFromDateFilterUse(bool b)
    {
        _filter.UseFromDateFilter = b;
    }
    
    public void SetToDateFilter(DateTime? date)
    {
        _filter.ToDateFilter = date;
    }
    
    public void SetToDateFilterUse(bool b)
    {
        _filter.UseToDateFilter = b;
    }
}