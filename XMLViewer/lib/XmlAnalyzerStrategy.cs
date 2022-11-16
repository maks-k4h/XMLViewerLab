namespace XMLViewer.lib;

// Strategy
public abstract class XmlAnalyzerStrategy
{
    protected const string DateFormat = "dd.MM.yyyy";
    
    protected string FilePath;

    public void SetFilePath(string path)
    {
        FilePath = path;
    }

    public abstract List<Article> Analyze(ArticleFilter filter);
}

