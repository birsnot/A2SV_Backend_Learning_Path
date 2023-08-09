namespace Blog.Models;

public class Post : BaseEntity
{
    public Post()
    {
        Comments = new HashSet<Comment>();
    }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public virtual ICollection<Comment> Comments { get; set; }
}