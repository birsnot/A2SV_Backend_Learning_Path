using System.Text.Json.Serialization;

namespace Blog.Models;

public class Comment : BaseEntity
{
    public int PostId { get; set; }
    public string Text { get; set; } = "";
    
    [JsonIgnore]
    public virtual Post? Post { get; set; }
}