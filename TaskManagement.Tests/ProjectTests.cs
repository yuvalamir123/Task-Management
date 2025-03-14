using Microsoft.Extensions.Logging;
using Moq;
using Task_Management.Models;
using Task_Management.Services;

namespace TaskManagement.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        private ProjectService _projectService;
        private Mock<ILogger<ProjectService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ProjectService>>();
            _projectService = new ProjectService(_mockLogger.Object);
        }

        [Test]
        public void CreateProject_ShouldAddProject()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);

            Assert.IsNotNull(createdProject);
            Assert.AreEqual(project.Name, createdProject.Name);
            Assert.IsNotEmpty(createdProject.Id);
        }

        [Test]
        public void GetProjectById_ShouldReturnCorrectProject()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);

            var fetchedProject = _projectService.GetProjectById(createdProject.Id);

            Assert.IsNotNull(fetchedProject);
            Assert.AreEqual(createdProject.Id, fetchedProject.Id);
        }

        [Test]
        public void GetProjects_ShouldReturnPaginatedList()
        {
            _projectService.CreateProject(new Project { Name = "Project 1" });
            _projectService.CreateProject(new Project { Name = "Project 2" });
            _projectService.CreateProject(new Project { Name = "Project 3" });

            var projects = _projectService.GetProjects(1, 2);
            Assert.AreEqual(2, projects.Count);
        }

        [Test]
        public void UpdateProject_ShouldModifyExistingProject()
        {
            var project = new Project { Name = "Initial Name" };
            var createdProject = _projectService.CreateProject(project);
            createdProject.Name = "Updated Name";

            var result = _projectService.UpdateProject(createdProject.Id, createdProject);
            var updatedProject = _projectService.GetProjectById(createdProject.Id);

            Assert.IsTrue(result);
            Assert.AreEqual("Updated Name", updatedProject.Name);
        }

        [Test]
        public void DeleteProject_ShouldRemoveProject()
        {
            var project = new Project { Name = "Test Project" };
            var createdProject = _projectService.CreateProject(project);
            var result = _projectService.DeleteProject(createdProject.Id);

            Assert.IsTrue(result);
            Assert.IsNull(_projectService.GetProjectById(createdProject.Id));
        }
    }
}