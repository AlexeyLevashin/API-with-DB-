using Application.Interfaces;
using Applications.DTO.Posts;
using Applications.DTO.Posts.Requests;
using FirstRestAPI.Common.Enums;
using FirstRestAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;

[Route("api/posts")]
    public class PostController : BaseController
    {
        private IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
 
        [Authorize(Roles = "Reader,Author")]
        [HttpGet("")]    
        public async Task<IActionResult> GetPosts()
        {
            return Ok(Role == Roles.Reader
                ? await postService.GetPostsForReader()
                : await postService.GetPostsForAuthor(UserId));
        }
 
        [Authorize(Roles = "Author")]
        [HttpPost("")]    
        public async Task<IActionResult> AddPost(AddPostRequestDTO addNewPostRequestDto)
        {
            var res = await postService.AddPost(UserId, addNewPostRequestDto);
            return Ok(res);
        }
 
        [Authorize(Roles = "Author")]
        [HttpPut("{postId:int}")]    
        public async Task<IActionResult> EditPost(int postId, EditPostRequestDTO editPostRequestDto)
        {
            await postService.UpdatePost(UserId, postId, editPostRequestDto);
            return Ok();
        }

        [Authorize(Roles = "Author")]
        [HttpPatch("{postId:int}/status")]
        public async Task<IActionResult> PublicPost(int postId, ChangePostStatusDTO changePostStatusDto)
        {
            await postService.PublishPost(UserId, postId, changePostStatusDto);
            return Ok();
        }

        [Authorize(Roles = "Author")]
        [HttpPost("{postId:int}/images")]
        public async Task<IActionResult> AddImageToPost(int postId, IFormFile image)
        {
            await using var stream = image.OpenReadStream();
            var post = await postService.AddImageToPost(postId, image.FileName, stream);
            return Ok(post);
        }

        [Authorize(Roles = "Author")]
        [HttpDelete("{postId:int}/images/{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int postId, int imageId)
        {
            await postService.DeleteImageFromPost(postId, imageId, UserId);
            return Ok();
        }
    }