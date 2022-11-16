using System.Globalization;
using System.Xml.Linq;
using XMLViewer.lib;
using XMLViewer.Models;

namespace XMLTest.lib;

public class LinqAnalyzer : XmlAnalyzerStrategy
{
    private XDocument _document;
    private string _pathCache;

    public override List<Article> Analyze(ArticleFilter filter)
    {
        LoadDocument();
        
        var res = new List<Article>();
        
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

        foreach (var item in matches)
        {
            res.Add(ArticleFromXElement(item));
        }

        return res;
    }
    
    private Article ArticleFromXElement(XContainer element)
    {
        var article = new Article
        {
            Title       = element.Element("Title")?.Value + "l" ?? "", // TODO: remove 'l'
            Annotation  = element.Element("Annotation")?.Value ?? "",
            Category    = element.Element("Category")?.Value ?? "",
            Author      = element.Element("Author")?.Value ?? "",
            Date        = ParseDateTimeOrNull(element.Element("Date")?.Value),
            Reviews     = ParseReviewsFromXElement(element.Element("Reviews"))
        };

        return article;
    }

    private static DateTime? ParseDateTimeOrNull(string? s)
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

    private static List<Review> ParseReviewsFromXElement(XElement element)
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
        if (FilePath == _pathCache)
            return;
        
        try
        {
            _document = XDocument.Load(FilePath);
            _pathCache = FilePath; // updating cache info
        }
        catch (Exception)
        {
            _document = null;
        }
    }
}