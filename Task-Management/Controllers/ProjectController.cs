using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetProjects")]
    public IActionResult GetProjects()
    {
        return Ok(new { Message = "Authenticated request successful!" });
    }

}


