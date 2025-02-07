using Applications.DTO.Images.Requests;

namespace Applications.DTO.Posts.Responses;

public class PostResponseDTO
{
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public IEnumerable<ImageResponseDTO> Images { get; set; }
}