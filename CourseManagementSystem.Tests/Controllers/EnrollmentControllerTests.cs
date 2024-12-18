using CourseManagementSystem.API.Controllers;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineCourseManagementSystem.Tests.Controllers
{
    [TestFixture]
    public class EnrollmentsControllerTests
    {
        private Mock<IEnrollmentService> _enrollmentServiceMock;
        private EnrollmentsController _controller;

        [SetUp]
        public void Setup()
        {
            _enrollmentServiceMock = new Mock<IEnrollmentService>();
            _controller = new EnrollmentsController(_enrollmentServiceMock.Object);
        }

        [Test]
        public async Task GetEnrollments_ReturnsOkResult_WithListOfEnrollments()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment { CourseId = 1, StudentId = 1 },
                new Enrollment { CourseId = 2, StudentId = 2 }
            };
            _enrollmentServiceMock.Setup(service => service.GetAllEnrollmentsAsync()).ReturnsAsync(enrollments);

            // Act
            var result = await _controller.GetEnrollments();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedEnrollments = okResult.Value as List<Enrollment>;
            Assert.AreEqual(2, returnedEnrollments.Count);
        }

        [Test]
        public async Task GetEnrollments_Returns500StatusCode_OnException()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.GetAllEnrollmentsAsync()).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetEnrollments();

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [Test]
        public async Task GetEnrollment_ReturnsOkResult_WithEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            _enrollmentServiceMock.Setup(service => service.GetEnrollmentByIdAsync(1, 1)).ReturnsAsync(enrollment);

            // Act
            var result = await _controller.GetEnrollment(1, 1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(enrollment, okResult.Value);
        }

        [Test]
        public async Task GetEnrollment_Returns404StatusCode_WhenEnrollmentNotFound()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.GetEnrollmentByIdAsync(1, 1)).ReturnsAsync((Enrollment)null);

            // Act
            var result = await _controller.GetEnrollment(1, 1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task EnrollStudent_ReturnsCreatedResult_WithNewEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            _enrollmentServiceMock.Setup(service => service.EnrollStudentAsync(It.IsAny<Enrollment>())).ReturnsAsync(enrollment);

            // Act
            var result = await _controller.EnrollStudent(enrollment);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(enrollment, createdResult.Value);
        }

        [Test]
        public async Task EnrollStudent_Returns400StatusCode_OnArgumentNullException()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.EnrollStudentAsync(It.IsAny<Enrollment>())).ThrowsAsync(new ArgumentNullException("enrollment"));

            // Act
            var result = await _controller.EnrollStudent(new Enrollment());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateEnrollment_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            _enrollmentServiceMock.Setup(service => service.UpdateEnrollmentAsync(It.IsAny<Enrollment>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEnrollment(1, 1, enrollment);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task UpdateEnrollment_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 2, StudentId = 2 };

            // Act
            var result = await _controller.UpdateEnrollment(1, 1, enrollment);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateEnrollmentProgress_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.UpdateEnrollmentProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEnrollmentProgress(1, 1, 50);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteEnrollment_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.DeleteEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEnrollment(1, 1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteEnrollment_Returns404StatusCode_OnKeyNotFoundException()
        {
            // Arrange
            _enrollmentServiceMock.Setup(service => service.DeleteEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException("Enrollment not found"));

            // Act
            var result = await _controller.DeleteEnrollment(1, 1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
