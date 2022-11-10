namespace XMLViewer.lib;

// Strategy
public interface IAnalyzer
{
    public List<Models.Article> Analyze(string filePath);
}

