using Application.Interfaces;
using Applications.DTO.Posts;
using Applications.DTO.Posts.Requests;
using Applications.DTO.Posts.Responses;
using FirstRestAPI.Mappers;
using Infrastructure.Models;
using Infrastructure.Models.InterfacesRepositories;


namespace FirstRestAPI.Application.Services;

using System.Threading.Tasks;


public class PostService : IPostService
{
    private readonly IPostRepository postRepository;
    
    public PostService(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    public async Task<List<PostResponseDTO>> GetPostsForReader()
    {
        // return (await postRepository.GetAllPublishedPosts()).MapToDomain().MapToDto();
        return null;
    }

    public async Task<List<PostResponseDTO>> GetPostsForAuthor(int userId)
    {
        // return (await postRepository.GetAllPosts(userId)).MapToDomain().MapToDto();
        return null;
    }

    public async Task<PostResponseDTO> AddPost(int authorId, AddPostRequestDTO addNewPostRequestDto)
    {
        if (await postRepository.GetPostByIdempotencyKey(addNewPostRequestDto.IdempotencyKey) != null)
        {
            throw new Exception("Пост с данным ключом уже существует");
        }
        
        return (await postRepository.AddPost(
            new Post
            {
                Content = addNewPostRequestDto.Content,
                Title = addNewPostRequestDto.Title,
                AuthorId = authorId,
                IdempotencyKey = addNewPostRequestDto.IdempotencyKey
            })).MapToDomain().MapToDto();
    }

    public async Task UpdatePost(int userId, int postId, EditPostRequestDTO editPostRequestDto)
    {
        var post = await GetPostByIdOrException(postId);

        if (post.AuthorId != userId)
        {
            throw new Exception("Вы не можете редактировать данный пост");
        }
        
        await postRepository.UpdatePost(new Post
        {
            PostId = postId, Content = editPostRequestDto.Content, Title = editPostRequestDto.Title
        });
    }

    public async Task PublishPost(int userId, int postId, ChangePostStatusDTO changePostStatusDto)
    {
        var res = await GetPostByIdOrException(postId);
        if (changePostStatusDto.IsPublished)
        {
            throw new Exception("Пост уже опубликован");
        }

        if (res.AuthorId != userId)
        {
            throw new Exception("Вы не можете редактировать данный пост");
        }

        await postRepository.PublishPost(postId);
    }

    public async Task<PostResponseDTO> AddImageToPost(int postId, string objectName, Stream image)
    {
        var post = await GetPostByIdOrException(postId);
        
        var uniqueObjectName =  $"{Guid.NewGuid()}-{objectName}";
        var dbImage = await postRepository.AddImageToPost(postId, uniqueObjectName);
        var res = post.MapToDomain();
        res.Images.Add(dbImage.MapToDomain());
        return res.MapToDto();
    }

    public async Task DeleteImageFromPost(int postId, int imageId, int userId)
    {
        var post = await GetPostByIdOrException(postId);

        if (post.AuthorId != userId)
        {
            throw new Exception("Вы не можете удалить фотографии в данном посте");
        }

        var image = post.Images?.FirstOrDefault(image => image.ImageId == imageId);
        if (image == null)
        {
            throw new Exception("Фотография поста не найдена");
        }
        
        await postRepository.DeleteImage(imageId);
    }

    private async Task<Post> GetPostByIdOrException(int postId)
    {
        var post = await postRepository.GetPostById(postId);
        if (post == null)
        {
            throw new Exception("Пост не найден");
        }

        return post;
    }
}