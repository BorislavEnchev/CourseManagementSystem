using CourseManagementSystem.API.Data;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Repositories
{
    [TestFixture]
    public class StudentRepositoryTests
    {
        private ApplicationDbContext _context;
        private StudentRepository _studentRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _studentRepository = new StudentRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };
            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();

            // Act
            var result = await _studentRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_StudentExists_ReturnsStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _studentRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_StudentDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _studentRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddAsync_AddsStudent_ReturnsStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _studentRepository.AddAsync(student);
            var addedStudent = await _context.Students.FindAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(student.FirstName, addedStudent.FirstName);
        }

        [Test]
        public async Task UpdateAsync_UpdatesStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            student.FirstName = "Updated John";

            // Act
            await _studentRepository.UpdateAsync(student);
            var updatedStudent = await _context.Students.FindAsync(1);

            // Assert
            Assert.AreEqual("Updated John", updatedStudent.FirstName);
        }

        [Test]
        public async Task DeleteAsync_StudentExists_DeletesStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            await _studentRepository.DeleteAsync(1);
            var deletedStudent = await _context.Students.FindAsync(1);

            // Assert
            Assert.IsNull(deletedStudent);
        }

        [Test]
        public async Task DeleteAsync_StudentDoesNotExist_NoOperation()
        {
            // Act
            await _studentRepository.DeleteAsync(1);

            // Assert
            var deletedStudent = await _context.Students.FindAsync(1);
            Assert.IsNull(deletedStudent);
        }
    }
}
