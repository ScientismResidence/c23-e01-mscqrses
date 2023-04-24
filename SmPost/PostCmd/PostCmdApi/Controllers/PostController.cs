using CqrsCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostCmdApi.Command;
using PostCmdApi.Dto;

namespace PostCmdApi.Controllers;

[ApiController]
[Route("api/v1/post")]
public class PostController : ControllerBase
{
    private readonly ICommandDispatcher _dispatcher;
    
    public PostController(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] AddPostCommand command)
    {
        Guid id = Guid.NewGuid();
        command.Id = id;
        await _dispatcher.SendAsync(command);
        
        return Ok(new PostAddedResponse
        {
            Id = id
        });
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostCommand command)
    {
        command.Id = id;
        await _dispatcher.SendAsync(command);
        
        return Ok();
    }

    [HttpPut("{id:guid}/like")]
    public async Task<IActionResult> LikePost(Guid id)
    {
        await _dispatcher.SendAsync(new LikePostCommand { Id = id });

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id, RemovePostCommand command)
    {
        command.Id = id;
        await _dispatcher.SendAsync(command);

        return Ok();
    }
    
    [HttpPut("{id:guid}/comment")]
    public async Task<IActionResult> CreateComment(Guid id, [FromBody] AddCommentCommand command)
    {
        command.Id = id;
        await _dispatcher.SendAsync(command);
        
        return Ok();
    }

    [HttpPut("{postId:guid}/comment/{commentId:guid}")]
    public async Task<IActionResult> UpdateComment(
        Guid postId, Guid commentId, [FromBody] UpdateCommentCommand command)
    {
        command.Id = postId;
        command.CommentId = commentId;
        await _dispatcher.SendAsync(command);

        return Ok();
    }

    [HttpDelete("{postId:guid}/comment/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(
        Guid postId, Guid commentId, [FromBody] RemoveCommentCommand command)
    {
        command.Id = postId;
        command.CommentId = commentId;
        await _dispatcher.SendAsync(command);

        return Ok();
    }
}