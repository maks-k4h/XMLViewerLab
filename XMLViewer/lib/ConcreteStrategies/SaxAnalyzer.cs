using System.Globalization;
using System.Text;
using System.Xml;

namespace XMLViewer.lib.ConcreteStrategies;

public class SaxAnalyzer : XmlAnalyzerStrategy
{
    public override List<Article> Analyze(ArticleFilter filter)
    {
        var result = new List<Article>();
        var reader = new XmlTextReader(FilePath);
        
        while (reader.Read())
        {
            if (reader.Name == "NewspaperData" && reader.NodeType == XmlNodeType.Element)
                ReadNewspaperData(result, reader, filter);
        }

        return result;
    }

    private void ReadNewspaperData(List<Article> articles, XmlTextReader reader, ArticleFilter filter)
    {
        while (reader.Read())
        {
            if (reader.Name == "Article" && reader.NodeType == XmlNodeType.Element)
                ReadArticle(articles, reader, filter);
        }
    }

    private void ReadArticle(List<Article> articles, XmlTextReader reader, ArticleFilter filter)
    {
        var article = new Article();
        while (reader.Read())
        {
            if (reader.Name == "Article" && reader.NodeType == XmlNodeType.EndElement)
                break;  // finished reading the article

            ReadAttribute(article, reader);
        }
        
        if (IsArticleAcceptable(article, filter))   // filtering
            articles.Add(article);
    }

    private void ReadAttribute(Article article, XmlTextReader reader)
    {
        switch (reader.Name)
        {
            case "Title":
                ReadTitle(article, reader);
                break;
            case "Annotation":
                ReadAnnotation(article, reader);
                break;
            case "Category":
                ReadCategory(article, reader);
                break;
            case "Author":
                ReadAuthor(article, reader);
                break;
            case "Date":
                ReadDate(article, reader);
                break;
            case "Reviews":
                ReadReviews(article, reader);
                break;
        }
    }

    private static void ReadTitle(Article article, XmlTextReader reader)
    {
        var stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Title" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        
        article.Title = stringBuilder + "s"; // TODO: remove 's'
    }
    
    private static void ReadAnnotation(Article article, XmlTextReader reader)
    {
        var stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Annotation" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }

        article.Annotation = stringBuilder.ToString();
    }
    
    private static void ReadCategory(Article article, XmlTextReader reader)
    {
        var stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Category" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        
        article.Category = stringBuilder.ToString();
    }
    
    private static void ReadAuthor(Article article, XmlTextReader reader)
    {
        var stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Author" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        
        article.Author = stringBuilder.ToString();
    }

    private static void ReadDate(Article article, XmlTextReader reader)
    {
        DateTime? date = null;
        
        while (reader.Read())
        {
            if (reader.Name == "Date" && reader.NodeType == XmlNodeType.EndElement)
                break;
            try
            {
                date = DateTime.ParseExact(reader.Value, DateFormat, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        article.Date = date;
    }

    private static void ReadReviews(Article article, XmlTextReader reader)
    {
        var reviews = new List<Review>();
        
        while (reader.Read())
        {
            if (reader.Name == "Reviews" && reader.NodeType == XmlNodeType.EndElement)
                break;
            if (reader.Name == "Review")
                ReadReview(reviews, reader);
        }

        article.Reviews = reviews;
    }

    private static void ReadReview(List<Review> reviews, XmlTextReader reader)
    {
        var stringBuilder = new StringBuilder();

        while (reader.Read())
        {
            if (reader.Name == "Review" && reader.NodeType == XmlNodeType.EndElement)
                break;

            stringBuilder.Append(reader.Value);
        }
        reviews.Add(new Review{ Text = stringBuilder.ToString() });
    }
    
    private static bool IsArticleAcceptable(Article article, ArticleFilter filter)
    {
        // title filter
        if (filter.UseTitleFilter 
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