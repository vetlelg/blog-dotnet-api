using Microsoft.EntityFrameworkCore;
namespace blog_dotnet_api.Models;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = null!;
}