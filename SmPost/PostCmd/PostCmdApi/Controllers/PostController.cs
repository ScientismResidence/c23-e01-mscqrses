using CqrsCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostCmdApi.Command;
using PostCmdApi.Dto;

namespace PostCmdApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class PostController : ControllerBase
{
    private readonly ICommandDispatcher _dispatcher;
    
    public PostController(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [HttpPost]
    [ActionName("post")]
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
}