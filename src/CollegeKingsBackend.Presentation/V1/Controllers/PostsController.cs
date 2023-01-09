using CollegeKings.Application.Posts;
using CollegeKingsBackend.Presentation.V1.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CollegeKingsBackend.Presentation.V1.Controllers;

public class PostsController : Controller
{
    [HttpGet(ApiRoutes.Posts.GetAll)]
    public IActionResult GetAll()
    {
        return Ok(PostsQueries.GetPosts());
    }
}