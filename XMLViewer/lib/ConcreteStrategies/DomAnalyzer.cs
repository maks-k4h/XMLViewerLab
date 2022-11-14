using System.Globalization;
using System.Xml;
using XMLTest.lib;
using XMLViewer.Models;

namespace XMLViewer.lib;

public class DomAnalyzer : XmlAnalyzerStrategy
{
    private XmlDocument _document = new XmlDocument();
    private string _pathCache = "";
    
    public override List<Article> Analyze(ArticleFilter filter)
    {
        LoadDocument(); // loading the document
        return ReadArticles(_document.DocumentElement, filter);
    }

    private List<Article> ReadArticles(XmlNode node, ArticleFilter filter)
    {
        var list = new List<Article>();
        if (node.Name == "NewspaperData")
        {
            foreach (XmlNode articleNode in node.ChildNodes)
            {
                var a = ReadArticle(articleNode, filter);
                if (a == null) continue;
                list.Add((Article)a);
            }
        }

        return list;
    }

    private Article? ReadArticle(XmlNode node, ArticleFilter filter)
    {
        Article article = new Article();
        foreach (XmlNode child in node.ChildNodes)
        {
            if (!ReadAttribute(child, article, filter))
                return null;
        }

        return article;
    }

    private bool ReadAttribute(XmlNode node, Article article, ArticleFilter filter)
    {
        switch (node.Name)
        {
            case "Title":
            {
                article.Title = node.InnerText + "d";   // TODO: remove 'd'
                if (filter.UseTitleFilter 
                    && filter.TitleFilter.Length > 0
                    && (article.Title.Length == 0 || !article.Title.ToLower().Contains(filter.TitleFilter)))
                    return false;
                break;
            }
            case "Annotation":
                article.Annotation = node.InnerText;
                break;
            case "Category":
            {
                article.Category = node.InnerText;
                if (filter.UseCategoryFilter 
                    && filter.CategoryFilter.Length > 0
                    && (article.Category.Length == 0 || !article.Category.ToLower().Contains(filter.CategoryFilter)))
                    return false;
                break;
            }
            case "Author":
            {
                article.Author = node.InnerText;
                if (filter.UseAuthorFilter 
                    && filter.AuthorFilter.Length > 0
                    && (article.Author.Length == 0 || !article.Author.ToLower().Contains(filter.AuthorFilter)))
                    return false;
                break;
            }
            case "Date":
            {
                article.Date = ReadDate(node);
                if (article.Date != null &&
                    (filter.UseFromDateFilter && article.Date < filter.FromDateFilter ||
                     filter.UseToDateFilter && article.Date > filter.ToDateFilter))
                    return false;
                break;
            }
            case "Reviews":
                article.Reviews = ReadReviews(node);
                break;
            default:
                // ignored
                break;
        }

        return true;
    }

    private List<Review> ReadReviews(XmlNode node)
    {
        var reviews = new List<Review>();
        foreach (XmlNode review in node.ChildNodes)
        {
            if (review.Name == "Review")
            {
                reviews.Add(new Review { Text = review.InnerText });
            }
        }

        return reviews;
    }

    private DateTime? ReadDate(XmlNode node)
    {
        DateTime? date;
        try
        {
            date = DateTime.ParseExact(node.InnerText, DateFormat, CultureInfo.CurrentCulture);
        }
        catch (Exception)
        {
            date = DateTime.Now;
        }

        return date;
    }

    private void LoadDocument()
    {
        if (_filePath == _pathCache)
            return;
        try
        {
            _document.Load(_filePath);
            _pathCache = _filePath; // updating cache info
        }
        catch (Exception)
        {
            // ignored
        }
        
    }
}