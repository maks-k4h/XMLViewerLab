using System.Globalization;
using System.Xml.Linq;
using XMLViewer.lib;
using XMLViewer.Models;

namespace XMLTest.lib;

public class LinqAnalyzer : XmlAnalyzerStrategy
{
    private XDocument? _document = null;
    private string _pathCache = null;

    public override List<Article> Analyze(ArticleFilter filter)
    {
        LoadDocument();
        List<Article> res = new List<Article>();
        
        if (_document is null)  // no exception is thrown
            return res;

        List<XElement> matches = _document.Descendants("NewspaperData")
            .Elements()
            // Title filter
            .Where(elem => 
                elem.Element("Title") != null 
                && (!filter.UseTitleFilter 
                    || elem.Element("Title")!.Value.ToLower().Contains(filter.TitleFilter)))
            // Category filter
            .Where(elem => 
                !filter.UseCategoryFilter 
                || filter.CategoryFilter.Length == 0 
                || (elem.Element("Category") != null 
                    && elem.Element("Category")!.Value.ToLower().Contains(filter.CategoryFilter)))
            // Author filter
            .Where(elem => 
                !filter.UseAuthorFilter 
                || filter.AuthorFilter.Length == 0 
                || (elem.Element("Author") != null 
                    && elem.Element("Author")!.Value.ToLower().Contains(filter.AuthorFilter)))
            // From date filter
            .Where(elem => 
                !filter.UseFromDateFilter
                || (elem.Element("Date") != null
                    && ParseDateTimeOrNull(elem.Element("Date")!.Value) != null
                    && ParseDateTimeOrNull(elem.Element("Date")!.Value) > filter.FromDateFilter))
            // To date filter
            .Where(elem => 
                !filter.UseToDateFilter
                || (elem.Element("Date") != null
                    && ParseDateTimeOrNull(elem.Element("Date")!.Value) != null
                    && ParseDateTimeOrNull(elem.Element("Date")!.Value) < filter.ToDateFilter))
            // executing and converting to a list
            .ToList();

        foreach (XElement item in matches)
        {
            res.Add(ArticleFromXElement(item));
        }

        return res;
    }
    
    private Article ArticleFromXElement(XElement element)
    {
        var article = new Article();

        article.Title       = element.Element("Title")?.Value + "l" ?? "";  // TODO: remove 'l'
        article.Annotation  = element.Element("Annotation")?.Value ?? "";
        article.Category    = element.Element("Category")?.Value ?? "";
        article.Author      = element.Element("Author")?.Value ?? "";
        article.Date        = ParseDateTimeOrNull(element.Element("Date")?.Value);
        article.Reviews     = ParseReviewsFromXElement(element.Element("Reviews"));
        
        return article;
    }

    private DateTime? ParseDateTimeOrNull(string? s)
    {
        try
        {
            if (s != null) 
                return DateTime.ParseExact(s, DateFormat, CultureInfo.CurrentCulture);
        }
        catch (Exception e)
        {
            // ignored
        }
        
        return null;
    }

    private List<Review> ParseReviewsFromXElement(XElement? element)
    {
        var res = new List<Review>();
        if (element is null)
            return res;

        foreach (var reviewElement in element.Elements("Review"))
        {
            res.Add(new Review{Text = reviewElement.Value});
        }

        return res;
    }

    private void LoadDocument()
    {
        if (_filePath == _pathCache)
            return;
        
        try
        {
            _document = XDocument.Load(_filePath);
            _pathCache = _filePath; // updating cache info
        }
        catch (Exception)
        {
            _document = null;
        }
    }
}