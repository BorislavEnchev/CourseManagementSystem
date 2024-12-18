using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Services;
using Moq;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Services
{
    [TestFixture]
    public class StudentServiceTests
    {
        private Mock<IStudentRepository> _studentRepositoryMock;
        private StudentService _studentService;

        [SetUp]
        public void Setup()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentService = new StudentService(_studentRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllStudentsAsync_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };
            _studentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(students);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetStudentByIdAsync_StudentExists_ReturnsStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

            // Act
            var result = await _studentService.GetStudentByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.FirstName);
        }

        [Test]
        public async Task GetStudentByIdAsync_StudentDoesNotExist_ReturnsNull()
        {
            // Arrange
            _studentRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Student)null);

            // Act
            var result = await _studentService.GetStudentByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateStudentAsync_ValidStudent_ReturnsCreatedStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentRepositoryMock.Setup(repo => repo.AddAsync(student)).ReturnsAsync(student);

            // Act
            var result = await _studentService.CreateStudentAsync(student);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.FirstName);
        }

        [Test]
        public async Task UpdateStudentAsync_ValidStudent_UpdatesStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            _studentRepositoryMock.Setup(repo => repo.UpdateAsync(student)).Returns(Task.CompletedTask);

            // Act
            await _studentService.UpdateStudentAsync(student);

            // Assert
            _studentRepositoryMock.Verify(repo => repo.UpdateAsync(student), Times.Once);
        }

        [Test]
        public async Task DeleteStudentAsync_ValidStudentId_DeletesStudent()
        {
            // Arrange
            _studentRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _studentService.DeleteStudentAsync(1);

            // Assert
            _studentRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
