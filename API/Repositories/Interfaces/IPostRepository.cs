

namespace Infrastructure.Models.InterfacesRepositories;

public interface IPostRepository
{
    public Task<List<Post>> GetAllPublishedPosts();
    public Task<Post?> GetPostById(int id);
    public Task<Post?> GetPostByIdempotencyKey(string idempotencyKey);
    public Task<List<Post>> GetAllPosts(int userId);
    public Task<Post> AddPost(Post post);
    public Task UpdatePost(Post post);
    public Task DeleteImage(int imageId);
    public Task PublishPost(int id);
    public Task<Image> AddImageToPost(int postId, string imageName);
}