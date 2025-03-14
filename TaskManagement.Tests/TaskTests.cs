using Microsoft.Extensions.Logging;
using Moq;
using Task_Management.Models;
using Task_Management.Services;

namespace TaskManagement.Tests
{
    [TestFixture]
    public class TaskTests
    {
        private TaskService _taskService;
        private ProjectService _projectService;
        private Mock<ILogger<TaskService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<TaskService>>();
            _projectService = new ProjectService(new Mock<ILogger<ProjectService>>().Object);
            _taskService = new TaskService(_mockLogger.Object, _projectService);
        }

        [Test]
        public void CreateTask_ShouldAddTaskToProject()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            var task = new TaskItem { Title = "Test Task" };
            var createdTask = _taskService.CreateTask(createdProject.Id, task);

            Assert.IsNotNull(createdTask);
            Assert.AreEqual(task.Title, createdTask.Title);
            Assert.IsNotEmpty(createdTask.Id);
        }

        [Test]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            var task = new TaskItem { Title = "Test Task" };
            var createdTask = _taskService.CreateTask(createdProject.Id, task);

            var fetchedTask = _taskService.GetTaskById(createdProject.Id, createdTask.Id);

            Assert.IsNotNull(fetchedTask);
            Assert.AreEqual(createdTask.Id, fetchedTask.Id);
        }

        [Test]
        public void GetTasks_ShouldReturnPaginatedTasks()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            _taskService.CreateTask(createdProject.Id, new TaskItem { Title = "Task 1" });
            _taskService.CreateTask(createdProject.Id, new TaskItem { Title = "Task 2" });

            var tasks = _taskService.GetTasks(createdProject.Id, 1, 1);
            Assert.AreEqual(1, tasks.Count);
        }

        [Test]
        public void UpdateTask_ShouldModifyExistingTask()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            var task = new TaskItem { Title = "Initial Title" };
            var createdTask = _taskService.CreateTask(createdProject.Id, task);
            createdTask.Title = "Updated Title";

            var result = _taskService.UpdateTask(createdProject.Id, createdTask.Id, createdTask);
            var updatedTask = _taskService.GetTaskById(createdProject.Id, createdTask.Id);

            Assert.IsTrue(result);
            Assert.AreEqual("Updated Title", updatedTask.Title);
        }

        [Test]
        public void DeleteTask_ShouldRemoveTaskFromProject()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            var task = new TaskItem { Title = "Test Task" };
            var createdTask = _taskService.CreateTask(createdProject.Id, task);
            var result = _taskService.DeleteTask(createdProject.Id, createdTask.Id);

            Assert.IsTrue(result);
            Assert.IsNull(_taskService.GetTaskById(createdProject.Id, createdTask.Id));
        }
    }
}
