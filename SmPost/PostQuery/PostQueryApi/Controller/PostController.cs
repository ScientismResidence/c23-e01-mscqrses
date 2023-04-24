using CqrsCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostQueryApi.Dto;
using PostQueryApi.Query;
using PostQueryDomain.Entity;

namespace PostQueryApi.Controller;

[ApiController]
[Route("api/v1/post")]
public class PostController : ControllerBase
{
    private readonly IQueryDispatcher<PostEntity> _dispatcher;
    
    public PostController(IQueryDispatcher<PostEntity> dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<PostEntity> posts = await _dispatcher.SendAsync(new GetAllPostsQuery());
        
        return Ok(new PostResponse
        {
            Posts = posts
        });
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        List<PostEntity> posts = await _dispatcher.SendAsync(new GetPostByIdQuery { Id = id });
        
        return Ok(new PostResponse
        {
            Posts = posts
        });
    }
    
    [HttpGet("author/{author}")]
    public async Task<IActionResult> GetPostsByAuthor(string author)
    {
        List<PostEntity> posts = await _dispatcher.SendAsync(new GetPostsByAuthorQuery { Author = author });
        
        return Ok(new PostResponse
        {
            Posts = posts
        });
    }

    [HttpGet("with-comments")]
    public async Task<IActionResult> GetPostsWithComments()
    {
        List<PostEntity> posts = await _dispatcher.SendAsync(new GetPostsWithCommentsQuery());
        
        return Ok(new PostResponse
        {
            Posts = posts
        });
    }
    
    [HttpGet("likes-at-least/{likes:int}")]
    public async Task<IActionResult> GetPostsWithLikes(int likes)
    {
        List<PostEntity> posts = await _dispatcher.SendAsync(new GetPostsWithLikesQuery { Likes = likes });
        
        return Ok(new PostResponse
        {
            Posts = posts
        });
    }
}