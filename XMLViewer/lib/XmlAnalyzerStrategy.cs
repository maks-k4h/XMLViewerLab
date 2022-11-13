using XMLTest.lib;

namespace XMLViewer.lib;

// Strategy
public abstract class XmlAnalyzerStrategy
{
    protected string _filePath;

    public void SetFilePath(string path)
    {
        _filePath = path;
    }

    public abstract List<Models.Article> Analyze(ArticleFilter filter);
}

