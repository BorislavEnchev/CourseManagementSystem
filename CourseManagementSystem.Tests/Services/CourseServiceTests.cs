using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Services;
using CourseManagementSystem.API.Factories.Interfaces;
using Moq;
using NUnit.Framework;

namespace CourseManagementSystem.Tests.Services
{
    [TestFixture]
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ICourseFactory> _courseFactoryMock;
        private CourseService _courseService;

        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseFactoryMock = new Mock<ICourseFactory>();
            _courseService = new CourseService(_courseRepositoryMock.Object, _courseFactoryMock.Object);
        }

        [Test]
        public async Task GetAllCoursesAsync_ReturnsAllCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Title = "Course 1" },
                new Course { Id = 2, Title = "Course 2" }
            };
            _courseRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);

            // Act
            var result = await _courseService.GetAllCoursesAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetCourseByIdAsync_CourseExists_ReturnsCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1" };
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(course);

            // Act
            var result = await _courseService.GetCourseByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Course 1", result.Title);
        }

        [Test]
        public async Task GetCourseByIdAsync_CourseDoesNotExist_ReturnsNull()
        {
            // Arrange
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Course)null);

            // Act
            var result = await _courseService.GetCourseByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateCourseAsync_ValidCourseType_ReturnsCreatedCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "New Course" };
            _courseFactoryMock.Setup(factory => factory.CreateCourse("Online")).Returns(course);
            _courseRepositoryMock.Setup(repo => repo.AddAsync(course)).ReturnsAsync(course);

            // Act
            var result = await _courseService.CreateCourseAsync("Online");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New Course", result.Title);
        }

        [Test]
        public async Task UpdateCourseAsync_ValidCourse_UpdatesCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Updated Course" };
            _courseRepositoryMock.Setup(repo => repo.UpdateAsync(course)).Returns(Task.CompletedTask);

            // Act
            await _courseService.UpdateCourseAsync(course);

            // Assert
            _courseRepositoryMock.Verify(repo => repo.UpdateAsync(course), Times.Once);
        }

        [Test]
        public async Task DeleteCourseAsync_ValidCourseId_DeletesCourse()
        {
            // Arrange
            _courseRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _courseService.DeleteCourseAsync(1);

            // Assert
            _courseRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
