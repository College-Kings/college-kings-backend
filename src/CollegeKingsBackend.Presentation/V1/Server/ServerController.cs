using Microsoft.AspNetCore.Mvc;

namespace CollegeKingsBackend.Presentation.V1.Server;

public class ServerController : Controller
{
    [HttpGet(ApiRoutes.Server.GetStatus)]
    public IActionResult GetStatus()
    {
        return Ok();
    }
}