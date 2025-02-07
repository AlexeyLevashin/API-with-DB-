namespace Applications.DTO.Posts.Requests;

public class AddPostRequestDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string IdempotencyKey { get; set; }
}