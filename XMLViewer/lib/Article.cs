namespace XMLViewer.lib;

public class Article
{
    public string       Title;
    public string       Author;
    public string       Annotation;
    public string       Category;
    public DateTime?    Date;

    public List<Review> Reviews = new List<Review>();

    public Article()
    {
        Title       = "";
        Annotation  = "";
        Author      = "";
        Category    = "";
        Date        = null;
    }

    public Article(string title, string annotation, string author, string category, DateTime date)
    {
        Title       = title;
        Annotation  = annotation;
        Author      = author;
        Category    = category;
        Date        = date;
    }
}