using Task_Management.Models;

namespace Task_Management.Services
{
    public class ProjectService
    {
        private readonly ILogger<ProjectService> _logger;

        public static readonly Dictionary<string, Project> Projects = new();

        public ProjectService(ILogger<ProjectService> logger)
        {
            _logger = logger;
        }

        public List<Project> GetProjects(int page, int size)
        {
            return Projects.Values.Skip((page - 1) * size).Take(size).ToList();
        }

        public Project? GetProjectById(string projectId)
        {
            if (!Projects.TryGetValue(projectId, out var project))
            {
                _logger.LogWarning($"[GetProjectById] Project not found. ID: {projectId}");
                return null;
            }
            return project;
        }

        public Project CreateProject(Project project)
        {
            project.Id = Guid.NewGuid().ToString();
            Projects[project.Id] = project;
            _logger.LogInformation($"[CreateProject] Project created. ID: {project.Id}");
            return project;
        }

        public bool UpdateProject(string projectId, Project updatedProject)
        {
            if (!Projects.ContainsKey(projectId))
            {
                _logger.LogWarning($"[UpdateProject] Project not found. ID: {projectId}");
                return false;
            }

            updatedProject.Id = projectId;
            Projects[projectId] = updatedProject;
            _logger.LogInformation($"[UpdateProject] Project updated. ID: {projectId}");
            return true;
        }

        public bool DeleteProject(string projectId)
        {
            if (!Projects.Remove(projectId))
            {
                _logger.LogWarning($"[DeleteProject] Project not found. ID: {projectId}");
                return false;
            }

            _logger.LogInformation($"[DeleteProject] Project deleted. ID: {projectId}");
            return true;
        }
    }
}
