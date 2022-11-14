using XMLViewer.Models;

namespace XMLViewer.lib;

public class HtmlExporter
{
    public static void Export(List<Article> articles, string path)
    {
        File.WriteAllText(path, "Some html text will be here...");
    }
}