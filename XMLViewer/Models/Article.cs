namespace XMLViewer.Models;

public class Article
{
    public string Title;
    public string Author;
    public string Category;

    public Article(string title, string author, string category)
    {
        Title = title;
        Author = author;
        Category = category;
    }
}