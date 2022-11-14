using XMLTest.lib;

namespace XMLViewer.lib;

// Strategy
public abstract class XmlAnalyzerStrategy
{
    public const string DateFormat = "dd.MM.yyyy";
    
    protected string _filePath;

    public void SetFilePath(string path)
    {
        _filePath = path;
    }

    public abstract List<Models.Article> Analyze(ArticleFilter filter);
}

