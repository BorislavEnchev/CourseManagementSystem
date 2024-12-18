using CourseManagementSystem.API.Controllers;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Controllers
{
    [TestFixture]
    public class StudentsControllerTests
    {
        private Mock<IStudentService> _studentServiceMock;
        private StudentsController _controller;

        [SetUp]
        public void Setup()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _controller = new StudentsController(_studentServiceMock.Object);
        }

        [Test]
        public async Task GetStudents_ReturnsOkResult_WithListOfStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };
            _studentServiceMock.Setup(service => service.GetAllStudentsAsync()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetStudents();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedStudents = okResult.Value as List<Student>;
            Assert.AreEqual(2, returnedStudents.Count);
        }

        [Test]
        public async Task GetStudents_Returns500StatusCode_OnException()
        {
            // Arrange
            _studentServiceMock.Setup(service => service.GetAllStudentsAsync()).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetStudents();

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [Test]
        public async Task GetStudent_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentServiceMock.Setup(service => service.GetStudentByIdAsync(1)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudent(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(student, okResult.Value);
        }

        [Test]
        public async Task GetStudent_Returns404StatusCode_WhenStudentNotFound()
        {
            // Arrange
            _studentServiceMock.Setup(service => service.GetStudentByIdAsync(1)).ReturnsAsync((Student)null);

            // Act
            var result = await _controller.GetStudent(1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }



        [Test]
        public async Task CreateStudent_ReturnsCreatedResult_WithNewStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentServiceMock.Setup(service => service.CreateStudentAsync(It.IsAny<Student>())).ReturnsAsync(student);

            // Act
            var result = await _controller.CreateStudent(student);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(student, createdResult.Value);
        }

        [Test]
        public async Task CreateStudent_Returns400StatusCode_OnArgumentNullException()
        {
            // Arrange
            var exceptionMessage = "Value cannot be null. (Parameter 'student')";
            _studentServiceMock.Setup(service => service.CreateStudentAsync(It.IsAny<Student>())).ThrowsAsync(new ArgumentNullException("student", exceptionMessage));

            // Act
            var result = await _controller.CreateStudent(new Student());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }


        [Test]
        public async Task UpdateStudent_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentServiceMock.Setup(service => service.UpdateStudentAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStudent(1, student);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task UpdateStudent_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var student = new Student { Id = 2, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _controller.UpdateStudent(1, student);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task DeleteStudent_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _studentServiceMock.Setup(service => service.DeleteStudentAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteStudent_Returns404StatusCode_OnKeyNotFoundException()
        {
            // Arrange
            _studentServiceMock.Setup(service => service.DeleteStudentAsync(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException("Student not found"));

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
