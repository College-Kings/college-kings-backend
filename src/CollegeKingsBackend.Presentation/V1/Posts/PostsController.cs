using CollegeKingsBackend.Application.V1.Posts;
using Microsoft.AspNetCore.Mvc;

namespace CollegeKingsBackend.Presentation.V1.Posts;

public class PostsController : Controller
{
    [HttpGet(ApiRoutes.Posts.GetAll)]
    public IActionResult GetAll()
    {
        return Ok(PostsQueries.GetPosts());
    }
}