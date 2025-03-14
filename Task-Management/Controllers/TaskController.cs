using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Models;
using Task_Management.Services;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/projects/{projectId}/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly TaskService _taskService;

    public TaskController(ILogger<TaskController> logger, TaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateTask(string projectId, [FromBody] TaskItem task)
    {
        var createdTask = _taskService.CreateTask(projectId, task);

        if (createdTask == null)
            return BadRequest("Invalid task or project not found.");

        return CreatedAtRoute("GetTaskById", new { projectId, taskId = createdTask.Id }, createdTask);
    }

    [HttpGet] 
    public IActionResult GetTasks(string projectId, int page = 1, int size = 10)
    {
        var tasks = _taskService.GetTasks(projectId, page, size);

        if (tasks.Count == 0)
            return NotFound("Project not found or no tasks available.");

        return Ok(tasks);
    }

    [HttpGet("{taskId}", Name = "GetTaskById")] 
    public IActionResult GetTaskById(string projectId, string taskId)
    {
        var task = _taskService.GetTaskById(projectId, taskId);

        if (task == null)
            return NotFound("Task not found.");

        return Ok(task);
    }

    [HttpPut("{taskId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateTask(string projectId, string taskId, [FromBody] TaskItem task)
    {
        if (!_taskService.UpdateTask(projectId, taskId, task))
            return NotFound("Task or project not found.");

        return NoContent();
    }

    [HttpDelete("{taskId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteTask(string projectId, string taskId)
    {
        if (!_taskService.DeleteTask(projectId, taskId))
            return NotFound("Task or project not found.");

        return NoContent();
    }
}
