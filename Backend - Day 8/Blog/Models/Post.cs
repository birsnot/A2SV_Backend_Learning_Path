namespace Blog.Models;

public sealed class Post : BaseEntity
{
    public Post()
    {
        Comments = new HashSet<Comment>();
    }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public ICollection<Comment> Comments { get; set; }
}