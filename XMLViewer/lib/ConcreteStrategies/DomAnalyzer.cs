using System.Globalization;
using System.Xml;
using XMLTest.lib;
using XMLViewer.Models;

namespace XMLViewer.lib;

public class DomAnalyzer : XmlAnalyzerStrategy
{
    private readonly XmlDocument _document = new XmlDocument();
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
                if (a != null)
                    list.Add(a);
            }
        }

        return list;
    }

    private Article? ReadArticle(XmlNode node, ArticleFilter filter)
    {
        var article = new Article();
        
        foreach (XmlNode child in node.ChildNodes)
        {
            ReadAttribute(child, article);
        }
        
        return IsArticleAcceptable(article, filter) ? article : null;
    }

    private bool ReadAttribute(XmlNode node, Article article)
    {
        switch (node.Name)
        {
            case "Title":
            {
                article.Title = node.InnerText.Trim();
                break;
            }
            case "Annotation":
                article.Annotation = node.InnerText;
                break;
            case "Category":
            {
                article.Category = node.InnerText.Trim();
                break;
            }
            case "Author":
            {
                article.Author = node.InnerText.Trim();
                break;
            }
            case "Date":
            {
                article.Date = ReadDate(node);
                break;
            }
            case "Reviews":
                article.Reviews = ReadReviews(node);
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
                reviews.Add(new Review { Text = review.InnerText.Trim() });
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
            date = null;
        }

        return date;
    }

    // loads and caches the document if it's not cached yet
    private void LoadDocument()
    {
        if (FilePath == _pathCache)
            return;
        try
        {
            _document.Load(FilePath);
            _pathCache = FilePath; // updating cache info
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private bool IsArticleAcceptable(Article article, ArticleFilter filter)
    {
        // title filter
        if (article.Title.Length == 0 ||
            filter.UseTitleFilter 
            && filter.TitleFilter.Length > 0
            && (article.Title.Length == 0 || !article.Title.ToLower().Contains(filter.TitleFilter)))
            return false;
        // category filter
        if (filter.UseCategoryFilter 
            && filter.CategoryFilter.Length > 0
            && (article.Category.Length == 0 || !article.Category.ToLower().Contains(filter.CategoryFilter)))
            return false;
        // author filter
        if (filter.UseAuthorFilter 
            && filter.AuthorFilter.Length > 0
            && (article.Author.Length == 0 || !article.Author.ToLower().Contains(filter.AuthorFilter)))
            return false;
        // date filter
        if (article.Date != null &&
            (filter.UseFromDateFilter && article.Date < filter.FromDateFilter ||
             filter.UseToDateFilter && article.Date > filter.ToDateFilter))
            return false;

        return true;    // passed all filters
    }
}