using Applications.DTO.Posts;
using Applications.DTO.Posts.Requests;
using Applications.DTO.Posts.Responses;
using Infrastructure.Models;

namespace Application.Interfaces;

public interface IPostService
{

    public Task<List<PostResponseDTO>> GetPostsForReader();
    public Task<List<PostResponseDTO>> GetPostsForAuthor(int userId);
    public Task<PostResponseDTO> AddPost(int authorId, AddPostRequestDTO addPostRequestDto);
    public Task UpdatePost(int userId, int postId, EditPostRequestDTO editPostRequestDto);
    public Task PublishPost(int userId, int postId, ChangePostStatusDTO changePostStatusDto);
    public Task<PostResponseDTO> AddImageToPost(int postId, string objectName, Stream image);
    public Task DeleteImageFromPost(int postId, int imageId, int userId);
}