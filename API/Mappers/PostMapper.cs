using Applications.DTO.Images.Requests;
using Applications.DTO.Posts.Responses;
using FirstRestAPI.Domain;
using Infrastructure.Models;
using FirstRestAPI.Mappers;

namespace FirstRestAPI.Mappers;

public static class PostMapper
{
    public static DomPost MapToDomain(this Post Post)
    {
        var p = new DomPost
        {
            PostId = Post.PostId,
            AuthorId = Post.AuthorId,
            IdempotencyKey = Post.IdempotencyKey,
            Title = Post.Title,
            Content = Post.Content,
            CreatedAt = Post.CreatedAt,
            UpdatedAt = Post.UpdatedAt,
            IsPublished = Post.IsPublished,
            Images = (Post.Images != null && Post.Images.Count != 0) ? Post.Images.MapToDomain() : new List<DomImage>() 
        };
        return p;
    }

    public static List<DomPost> MapToDomain(this List<Post> Posts)
    {
        return Posts.Select(i => i.MapToDomain()).ToList();
    }

    public static PostResponseDTO MapToDto(this DomPost post)
    {
        return new PostResponseDTO
        {
            PostId = post.PostId,
            AuthorId = post.AuthorId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            IsPublished = post.IsPublished,
            Images = post.Images.MapToDto() 
        };
    }
    
    
    public static List<PostResponseDTO> MapToDto(List<DomPost> posts)
    {
        return posts.Select(post => post.MapToDto()).ToList();
    }
}