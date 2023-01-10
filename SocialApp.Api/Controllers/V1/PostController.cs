using Microsoft.AspNetCore.Mvc;


namespace SocialApp.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]

public class PostController : Controller
{
    public PostController()
    {
        
    }
    [HttpGet]
    [Route(ApiRoutes.Post.GetById)]
    public IActionResult GetById(int id)
    {
        return Ok();
    }
}