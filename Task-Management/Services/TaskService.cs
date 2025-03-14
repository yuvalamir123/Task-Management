using Task_Management.Models;

namespace Task_Management.Services
{
    public class TaskService
    {
        private readonly ILogger<TaskService> _logger;
        private readonly ProjectService _projectService;

        public TaskService(ILogger<TaskService> logger, ProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public TaskItem? CreateTask(string projectId, TaskItem task)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project == null) return null;

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                _logger.LogWarning("[CreateTask] Invalid task data received.");
                return null;
            }

            task.Id = Guid.NewGuid().ToString();
            project.Tasks[task.Id] = task;
            _logger.LogInformation($"[CreateTask] Task created. ID: {task.Id}, Project: {projectId}");

            return task;
        }

        public List<TaskItem> GetTasks(string projectId, int page, int size)
        {
            var project = _projectService.GetProjectById(projectId);
            return project?.Tasks.Values.Skip((page - 1) * size).Take(size).ToList() ?? new List<TaskItem>();
        }

        public TaskItem? GetTaskById(string projectId, string taskId)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project == null || !project.Tasks.ContainsKey(taskId)) return null;

            return project.Tasks[taskId];
        }

        public bool UpdateTask(string projectId, string taskId, TaskItem task)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project == null || !project.Tasks.ContainsKey(taskId)) return false;

            task.Id = taskId;
            project.Tasks[taskId] = task;
            _logger.LogInformation($"[UpdateTask] Task updated. ID: {taskId}, Project: {projectId}");

            return true;
        }

        public bool DeleteTask(string projectId, string taskId)
        {
            var project = _projectService.GetProjectById(projectId);
            if (project == null || !project.Tasks.TryRemove(taskId, out _)) return false;

            _logger.LogInformation($"[DeleteTask] Task deleted. ID: {taskId}, Project: {projectId}");
            return true;
        }
    }
}
