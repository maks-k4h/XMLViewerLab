namespace XMLViewer.Models;

public struct Article
{
    public string Title;
    public string Author;
    public string Annotation;
    public string Category;

    public List<Review> Reviews = new List<Review>();

    public Article()
    {
        Title       = "empty";
        Annotation  = "empty";
        Author      = "empty";
        Category    = "empty";
    }

    public Article(string title, string notation, string author, string category)
    {
        Title       = title;
        Author      = author;
        Category    = category;
    }
}