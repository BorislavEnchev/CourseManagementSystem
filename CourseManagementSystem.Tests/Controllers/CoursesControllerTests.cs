using CourseManagementSystem.API.Controllers;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CourseManagementSystem.Tests.Controllers
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private Mock<ICourseService> _courseServiceMock;
        private CoursesController _coursesController;
        private readonly WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _coursesController = new CoursesController(_courseServiceMock.Object);
        }

        [Test]
        public async Task GetCourses_ReturnsOkResult_WhenCoursesExist()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Title = "Course 1" },
                new Course { Id = 2, Title = "Course 2" }
            };
            _courseServiceMock.Setup(service => service.GetAllCoursesAsync()).ReturnsAsync(courses);

            // Act
            var result = await _coursesController.GetCourses();

            // Assert
            var okResult = result as ActionResult<IEnumerable<Course>>;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Result as OkObjectResult;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(200, returnValue.StatusCode);
            var coursesReturned = returnValue.Value as List<Course>;
            Assert.AreEqual(2, coursesReturned.Count);
        }


        [Test]
        public async Task GetCourse_ReturnsOkResult_WhenCourseExists()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1" };
            _courseServiceMock.Setup(service => service.GetCourseByIdAsync(1)).ReturnsAsync(course);

            // Act
            var result = await _coursesController.GetCourse(1);

            // Assert
            var okResult = result as ActionResult<Course>;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Result as OkObjectResult;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(200, returnValue.StatusCode);
            var courseReturned = returnValue.Value as Course;
            Assert.AreEqual("Course 1", courseReturned.Title);
        }       


        [Test]
        public async Task CreateCourse_ReturnsCreatedAtActionResult_WhenCourseIsCreated()
        {
            // Arrange
            var newCourse = new Course { Id = 1, Title = "New Course" };
            _courseServiceMock.Setup(service => service.CreateCourseAsync("Online")).ReturnsAsync(newCourse);

            // Act
            var result = await _coursesController.CreateCourse("Online");

            // Assert
            var createdAtActionResult = result as ActionResult<Course>;
            Assert.IsNotNull(createdAtActionResult);

            // Check that the result is CreatedAtActionResult
            var returnValue = createdAtActionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(returnValue);

            // Assert that the status code is 201 (Created)
            Assert.AreEqual(201, returnValue.StatusCode);

            // Assert that the course data returned is the same as the created course
            var courseReturned = returnValue.Value as Course;
            Assert.IsNotNull(courseReturned);
            Assert.AreEqual("New Course", courseReturned.Title);
        }


        [Test]
        public async Task UpdateCourse_ReturnsNoContent_WhenCourseIsUpdated()
        {
            // Arrange
            var updatedCourse = new Course { Id = 1, Title = "Updated Course" };
            _courseServiceMock.Setup(service => service.UpdateCourseAsync(updatedCourse)).Returns(Task.CompletedTask);

            // Act
            var result = await _coursesController.UpdateCourse(1, updatedCourse);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteCourse_ReturnsNoContent_WhenCourseIsDeleted()
        {
            // Arrange
            _courseServiceMock.Setup(service => service.DeleteCourseAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _coursesController.DeleteCourse(1);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
    }
}
