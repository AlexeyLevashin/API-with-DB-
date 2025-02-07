using Infrastructure.Models;

namespace FirstRestAPI.Domain;

public class DomPost
{
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string IdempotencyKey { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public List<DomImage> Images { get; set; }
}