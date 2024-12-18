using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Services;
using Moq;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Services
{
    [TestFixture]
    public class EnrollmentServiceTests
    {
        private Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
        private Mock<ICourseRepository> _courseRepositoryMock;
        private EnrollmentService _enrollmentService;

        [SetUp]
        public void Setup()
        {
            _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _enrollmentService = new EnrollmentService(_enrollmentRepositoryMock.Object, _courseRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllEnrollmentsAsync_ReturnsAllEnrollments()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment { CourseId = 1, StudentId = 1 },
                new Enrollment { CourseId = 2, StudentId = 2 }
            };
            _enrollmentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enrollments);

            // Act
            var result = await _enrollmentService.GetAllEnrollmentsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetEnrollmentByIdAsync_EnrollmentExists_ReturnsEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            _enrollmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1, 1)).ReturnsAsync(enrollment);

            // Act
            var result = await _enrollmentService.GetEnrollmentByIdAsync(1, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
        }

        [Test]
        public async Task GetEnrollmentByIdAsync_EnrollmentDoesNotExist_ReturnsNull()
        {
            // Arrange
            _enrollmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1, 1)).ReturnsAsync((Enrollment)null);

            // Act
            var result = await _enrollmentService.GetEnrollmentByIdAsync(1, 1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task EnrollStudentAsync_ValidEnrollment_EnrollsStudentAndUpdateCourseSeats()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            var course = new Course { Id = 1, SeatsAvailable = 30 };
            _enrollmentRepositoryMock.Setup(repo => repo.AddAsync(enrollment)).ReturnsAsync(enrollment);
            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(course);
            _courseRepositoryMock.Setup(repo => repo.UpdateAsync(course)).Returns(Task.CompletedTask);

            // Act
            var result = await _enrollmentService.EnrollStudentAsync(enrollment);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(29, course.SeatsAvailable); // Seats should be decreased by 1
            _courseRepositoryMock.Verify(repo => repo.UpdateAsync(course), Times.Once);
        }

        [Test]
        public async Task UpdateEnrollmentProgressAsync_ValidProgress_UpdatesProgressAndCompletion()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1, Progress = 50, IsCompleted = false };
            _enrollmentRepositoryMock.Setup(repo => repo.GetByIdAsync(1, 1)).ReturnsAsync(enrollment);
            _enrollmentRepositoryMock.Setup(repo => repo.UpdateAsync(enrollment)).Returns(Task.CompletedTask);

            // Act
            await _enrollmentService.UpdateEnrollmentProgressAsync(1, 1, 100);

            // Assert
            Assert.AreEqual(100, enrollment.Progress);
            Assert.IsTrue(enrollment.IsCompleted);
            _enrollmentRepositoryMock.Verify(repo => repo.UpdateAsync(enrollment), Times.Once);
        }

        [Test]
        public async Task UpdateEnrollmentAsync_ValidEnrollment_UpdatesEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1 };
            _enrollmentRepositoryMock.Setup(repo => repo.UpdateAsync(enrollment)).Returns(Task.CompletedTask);

            // Act
            await _enrollmentService.UpdateEnrollmentAsync(enrollment);

            // Assert
            _enrollmentRepositoryMock.Verify(repo => repo.UpdateAsync(enrollment), Times.Once);
        }

        [Test]
        public async Task DeleteEnrollmentAsync_ValidEnrollmentId_DeletesEnrollment()
        {
            // Arrange
            _enrollmentRepositoryMock.Setup(repo => repo.DeleteAsync(1, 1)).Returns(Task.CompletedTask);

            // Act
            await _enrollmentService.DeleteEnrollmentAsync(1, 1);

            // Assert
            _enrollmentRepositoryMock.Verify(repo => repo.DeleteAsync(1, 1), Times.Once);
        }
    }
}
