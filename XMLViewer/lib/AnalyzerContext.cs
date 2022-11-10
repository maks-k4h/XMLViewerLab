using XMLViewer.Models;

namespace XMLViewer.lib;

// The context for simple interaction with strategies
public class AnalyzerContext
{
    private IAnalyzer _analyzer;
    private string _filePath;
    
    public enum XmlAnalysisStrategy
    {
        Dom,
        Linq,
        Sax
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
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }
    }

    public void SetFilePath(string path)
    {
        _filePath = path;
    }

    public List<Article> Run()
    {
        return _analyzer.Analyze(_filePath);
    }
}