using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Post
{
    public int PostId { get; set; }
    [Required] public int AuthorId { get; set; }
    [Required] public string IdempotencyKey { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public List<Image>? Images { get; set; }
}