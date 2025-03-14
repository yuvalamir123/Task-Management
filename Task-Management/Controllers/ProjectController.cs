using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Models;
using Task_Management.Services;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly ProjectService _projectService;

    public ProjectController(ILogger<ProjectController> logger, ProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    [HttpGet(Name = "GetProjects")]
    public IActionResult GetProjects(int page = 1, int size = 10)
    {
        var projects = _projectService.GetProjects(page, size);
        return Ok(projects);
    }

    [HttpGet("{projectId}", Name = "GetProjectById")]
    public IActionResult GetProjectById(string projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        return project != null ? Ok(project) : NotFound("Project not found.");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateProject([FromBody] Project project)
    {
        if (project == null || string.IsNullOrWhiteSpace(project.Name))
        {
            _logger.LogWarning("[CreateProject] Invalid project data received.");
            return BadRequest("Valid project data is required.");
        }

        var createdProject = _projectService.CreateProject(project);
        return CreatedAtRoute("GetProjectById", new { projectId = createdProject.Id }, createdProject);
    }

    [HttpPut("{projectId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProject(string projectId, [FromBody] Project project)
    {
        if (!_projectService.UpdateProject(projectId, project))
            return NotFound("Project not found.");

        return NoContent();
    }

    [HttpDelete("{projectId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteProject(string projectId)
    {
        if (!_projectService.DeleteProject(projectId))
            return NotFound("Project not found.");

        return NoContent();
    }
}
