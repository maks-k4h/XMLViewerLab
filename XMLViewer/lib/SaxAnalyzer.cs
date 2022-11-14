using System.Text;
using System.Xml;
using XMLTest.lib;
using XMLViewer.Models;

namespace XMLViewer.lib;

public class SaxAnalyzer : XmlAnalyzerStrategy
{
    public override List<Article> Analyze(ArticleFilter filter)
    {
        List<Article> result = new List<Article>();

        var reader = new XmlTextReader(_filePath);
        while (reader.Read())
        {
            if (reader.Name == "NewspaperData" && reader.NodeType == XmlNodeType.Element)
            {
                ReadNewspaperData(result, reader, filter);
            }
        }

        return result;
    }

    private void ReadNewspaperData(List<Article> articles, XmlTextReader reader, ArticleFilter filter)
    {
        while (reader.Read())
        {
            if (reader.Name == "Article" && reader.NodeType == XmlNodeType.Element)
            {
                ReadArticle(articles, reader, filter);
            }
        }
    }

    private void ReadArticle(List<Article> articles, XmlTextReader reader, ArticleFilter filter)
    {
        Article article = new Article();
        while (reader.Read())
        {
            if (reader.Name == "Article" && reader.NodeType == XmlNodeType.EndElement)
                break;

            if (reader.NodeType != XmlNodeType.Element) 
                continue;
            
            switch (reader.Name)
            {
                case "Title":
                {
                    if (!ReadTitle(article, reader, filter))
                        return;
                    break;
                }
                case "Annotation":
                    // no annotations filters — no need to check validity
                    ReadAnnotation(article, reader);
                    break;
                case "Category":
                {
                    if (!ReadCategory(article, reader, filter))
                        return;
                    break;
                }
                case "Author":
                {
                    if (!ReadAuthor(article, reader, filter))
                        return;
                    break;
                }
                case "Date":
                {
                    if (!ReadDate(article, reader, filter))
                        return;
                    break;
                }
                case "Reviews":
                    // no reviews filters — no need to check validity
                    ReadReviews(article, reader);
                    break;
                default:
                    // ignored
                    break;
            }
        }
        
        // finally, adding the article
        articles.Add(article);
    }

    private bool ReadTitle(Article article, XmlTextReader reader, ArticleFilter filter)
    {
        StringBuilder stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Title" && reader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }

            stringBuilder.Append(reader.Value);
        }
        
        article.Title = stringBuilder.ToString();
        
        // filtering
        if (filter.UseTitleFilter && 
            !article.Title.ToLower().Contains(filter.TitleFilter) || article.Title.Length == 0)
            return false;
        
        return true;
    }
    
    private void ReadAnnotation(Article article, XmlTextReader reader)
    {
        StringBuilder stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Annotation" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }

        article.Annotation = stringBuilder.ToString();
    }
    
    private bool ReadCategory(Article article, XmlTextReader reader, ArticleFilter filter)
    {
        StringBuilder stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Category" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        
        article.Category = stringBuilder.ToString();
        
        // filtering
        if (filter.UseCategoryFilter && filter.CategoryFilter.Length > 0 
            && !article.Category.ToLower().Contains(filter.CategoryFilter) || article.Category.Length == 0)
            return false;

        return true;
    }
    
    private bool ReadAuthor(Article article, XmlTextReader reader, ArticleFilter filter)
    {
        StringBuilder stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Author" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        
        article.Author = stringBuilder.ToString();
        
        // filtering
        if (filter.UseAuthorFilter && filter.AuthorFilter.Length > 0 
            && !article.Author.ToLower().Contains(filter.AuthorFilter) || article.Author.Length == 0)
            return false;

        return true;
    }

    private bool ReadDate(Article article, XmlTextReader reader, ArticleFilter filter)
    {
        DateTime? date = null;
        
        while (reader.Read())
        {
            if (reader.Name == "Date" && reader.NodeType == XmlNodeType.EndElement)
                break;
            try
            {
                date = DateTime.Parse(reader.Value);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        article.Date = date;
        
        // filtering
        if (filter.UseFromDateFilter && date < filter.FromDateFilter)
            return false;
        if (filter.UseToDateFilter && date > filter.ToDateFilter)
            return false;

        return true;
    }

    private void ReadReviews(Article article, XmlTextReader reader)
    {
        List<Review> reviews = new List<Review>();
        while (reader.Read())
        {
            if (reader.Name == "Reviews" && reader.NodeType == XmlNodeType.EndElement)
                break;
            if (reader.Name == "Review")
                ReadReview(reviews, reader);
        }

        article.Reviews = reviews;
    }

    private void ReadReview(List<Review> reviews, XmlTextReader reader)
    {
        StringBuilder stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Review" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        reviews.Add(new Review{ Text = stringBuilder.ToString() });
    }
}