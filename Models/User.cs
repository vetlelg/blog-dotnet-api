namespace blog_dotnet_api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>();
}