using System.Xml;
using XMLViewer.Models;

namespace XMLViewer.lib;

public class DomAnalyzer : IAnalyzer
{
    public List<Article> Analyze(string filePath)
    {
        var document = new XmlDocument();
        document.Load(filePath);
        
        return ReadArticles(document.DocumentElement);
    }

    private List<Article> ReadArticles(XmlNode node)
    {
        var list = new List<Article>();
        if (node.Name == "NewspaperData")
            foreach (XmlNode articleNode in node.ChildNodes)
            {
                list.Add(ReadArticle(articleNode));
            }

        return list;
    }

    private Article ReadArticle(XmlNode node)
    {
        Article article = new Article();
        if (node.Attributes != null)
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Title")
                {
                    article.Title = child.InnerText;
                }
                else if (child.Name == "Annotation")
                {
                    article.Annotation = child.InnerText;
                }
                else if (child.Name == "Category")
                {
                    article.Category = child.InnerText;
                }
                else if (child.Name == "Author")
                {
                    article.Author = child.InnerText;
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
}