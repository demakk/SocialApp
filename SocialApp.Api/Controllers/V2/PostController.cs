using Microsoft.AspNetCore.Mvc;
using Social.Domain.Models;

namespace SocialApp.Controllers.V2;

[ApiVersion("2.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[Controller]")]
public class PostController : Controller
{
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var post = new Post { Id = id, Text = "Hello universe" };
        return Ok(post);
    }
}