using Microsoft.AspNetCore.Mvc;
using Social.Domain.Models;

namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
[ApiController]

public class PostController : Controller
{
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var post = new Post { Id = id, Text = "Hello world" };
        return Ok(post);
    }
}