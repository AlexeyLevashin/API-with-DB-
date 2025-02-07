using Infrastructure.Dapper;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.InterfacesRepositories;


namespace Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IDapperContext dapperContext;

    public PostRepository(IDapperContext _dapperContext)
    {
        dapperContext = _dapperContext;
    }

    

    public async Task<Post> AddPost(Post post)
    {
        var queryObject = new QueryObject(
            "INSERT INTO POSTS(author_id, idempotency_key, title, content, created_at, updated_at, is_published) VALUES(@author_id, @idempotency_key, @title, @content, @created_at, @updated_at, 'false') RETURNING id as \"PostId\", author_id as \"AuthorId\", idempotency_key as \"IdempotencyKey\", title as \"Title\", content as \"Content\", created_at as \"CreatedAt\", updated_at as \"UpdatedAt\", is_published as \"IsPublished\"",
            new
            {
                author_Id = post.AuthorId, idempotency_key = post.IdempotencyKey, title = post.Title, content = post.Content, created_at = post.CreatedAt, updated_at = post.UpdatedAt, is_published = post.IsPublished
            });
        return await dapperContext.CommandWithResponse<Post>(queryObject);
    }
    
    public async Task<Post> GetPostById(int id)
    {
        var queryObject = new QueryObject(
            @"SELECT p.id, p.author_id as ""AuthorId"", idempotency_key as ""IdempotencyKey"", p.title as ""Title"", p.Content as ""Content"", p.created_at as ""CreatedAt"", p.updated_at as ""UpdatedAt"", p.IsPublished,
                   i.id, i.post_id as ""PostIdIm"", i.image_name as ""ImageName"", i.created_at as ""CreatedAt""
            FROM POSTS p
            LEFT JOIN IMAGES i ON p.PostId = i.post_id WHERE p.id = @post_id", new {post_id=id});
        var res = await GetPostWithImages(queryObject);
        return res.Count != 0 ? res[0] : null;
    }
    
    public async Task<Post?> GetPostByIdempotencyKey(string idempotencyKey)
    {
        var queryObject = new QueryObject("SELECT * FROM POSTS WHERE idempotency_key=@idempotencyKey", new {idempotencyKey});
        return await dapperContext.FirstOrDefault<Post>(queryObject);
    }
    public async Task<List<Post>> GetAllPublishedPosts()
    {
        var queryObject = new QueryObject(
            @"SELECT p.id, p.author_id as ""AuthorId"", idempotency_key as ""IdempotencyKey"", p.title as ""Title"", p.Content as ""Content"", p.created_at as ""CreatedAt"", p.updated_at as ""UpdatedAt"", p.IsPublished,
                   i.id, i.post_id as ""PostIdIm"", i.image_name as ""ImageName"", i.created_at as ""CreatedAt""
            FROM POSTS p
            LEFT JOIN IMAGES i ON p.PostId = i.post_id WHERE status = 'published'");
        var res = await GetPostWithImages(queryObject);
        return res;
    }

    public async Task<List<Post>> GetAllPosts(int userId)
    {
        var queryObject = new QueryObject(
            @"SELECT p.id, p.author_id as ""AuthorId"", idempotency_key as ""IdempotencyKey"", p.title as ""Title"", p.content as ""Content"", p.created_at as ""CreatedAt"", p.updated_at as ""UpdatedAt"", p.IsPublished,
                   i.id, i.post_id as ""PostIdIm"", i.image_name as ""ImageName"", i.created_at as ""CreatedAt""
            FROM POSTS p
            LEFT JOIN IMAGES i ON p.PostId = i.post_id where author_id=@author_id", new {author_id=userId});
        var res = await GetPostWithImages(queryObject);
        return res;
    }
    
    public async Task<Image> AddImageToPost(int postId, string imageName)
    {
        var queryObject = new QueryObject(
        "INSERT INTO IMAGES(post_id, image_name, created_at) VALUES(@postId, @imageName, now()) RETURNING id as \"ImageId\", post_id as \"PostIdIm\", image_name as \"ImageName\", created_at as \"CreatedAt\"",
        new {postId, imageName});
        return await dapperContext.CommandWithResponse<Image>(queryObject);
    }

    public async Task UpdatePost(Post post)
    {
        var queryObject = new QueryObject(
            "UPDATE POSTS SET title = @title, content = @content, updated_at = now() where id = @post_id",
            new { title = post.Title, content = post.Content, post_Id = post.PostId });
        await dapperContext.Command(queryObject);
    }

    public async Task DeleteImage(int image_id)
    {
        var queryObject = new QueryObject(
            "DELETE FROM IMAGES WHERE id = @image_id", new{image_id});
            

        await dapperContext.Command(queryObject);
    }

    public async Task PublishPost(int id)
    {
        var queryObject = new QueryObject(
            "UPDATE POSTS SET is_published = 'true' where id = @id",
            new {id});
        await dapperContext.Command(queryObject);
    }
    
    private async Task<List<Post>> GetPostWithImages(IQueryObject queryObject)
    {
        var dictionary = new Dictionary<int, Post>();
        var res = await dapperContext.QueryWithJoin<Post, Image?, Post>(queryObject, (post, image) =>
        {
            Post p;
            if (!dictionary.TryGetValue(post.PostId, out p))
            {
                p = post;
                p.Images = new List<Image>();
                dictionary.Add(p.PostId, p);
            }

            if (p.PostId == image?.PostIdIm)
            {
                p.Images?.Add(image);
            }

            return p;
        }, "id");

        return res.Distinct().ToList();
    }
}