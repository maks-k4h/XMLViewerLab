using System.Xml.Xsl;

namespace XMLViewer.lib;

public class HtmlExporter
{
    private static readonly string XslFilePath = "../lib/html/style.xsl";
    
    public static void Export(List<Article> articles, string path)
    {
        string appDataPath = Environment.CurrentDirectory;
        var transform = new XslCompiledTransform();
        transform.Load(XslFilePath);
        
        transform.Transform(Data.FileInfo.FilePath, path);
    }
}