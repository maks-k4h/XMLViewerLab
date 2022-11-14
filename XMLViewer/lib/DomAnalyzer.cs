using System.Xml;
using XMLTest.lib;
using XMLViewer.Models;

namespace XMLViewer.lib;

public class DomAnalyzer : XmlAnalyzerStrategy
{
    public override List<Article> Analyze(ArticleFilter filter)
    {
        var document = new XmlDocument();
        document.Load(_filePath);
        
        return ReadArticles(document.DocumentElement, filter);
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
            if (child.Name == "Title")
            {
                article.Title = child.InnerText;
                if (filter.UseTitleFilter && !article.Title.ToLower().Contains(filter.TitleFilter))
                    return null;
            }
            else if (child.Name == "Annotation")
            {
                article.Annotation = child.InnerText;
            }
            else if (child.Name == "Category")
            {
                article.Category = child.InnerText;
                if (filter.UseCategoryFilter && !article.Category.ToLower().Contains(filter.CategoryFilter))
                    return null;
            }
            else if (child.Name == "Author")
            {
                article.Author = child.InnerText;
                if (filter.UseAuthorFilter && !article.Author.ToLower().Contains(filter.AuthorFilter))
                    return null;
            }
            else if (child.Name == "Date")
            {
                article.Date = ReadDate(child);
                if (article.Date != null &&     // articles with no date are allowed
                    (filter.UseFromDateFilter && article.Date < filter.FromDateFilter ||
                     filter.UseToDateFilter && article.Date > filter.ToDateFilter))
                {
                    return null;
                }
                    
            }
            else if (child.Name == "Reviews")
            {
                article.Reviews = ReadReviews(child);
            }
        }

        return article;
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
            date = DateTime.Parse(node.InnerText);
        }
        catch (Exception)
        {
            date = DateTime.Now;
        }

        return date;
    }
}