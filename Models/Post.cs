namespace blog_dotnet_api.Models;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
    public User User { get; set; } = null!;
}