using CourseManagementSystem.API.Data;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Repositories
{
    [TestFixture]
    public class EnrollmentRepositoryTests
    {
        private ApplicationDbContext _context;
        private EnrollmentRepository _enrollmentRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _enrollmentRepository = new EnrollmentRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllEnrollments()
        {
            // Arrange
            var course1 = new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 };
            var course2 = new Course { Id = 2, Title = "Course 2", Description = "Description 2", Credits = 4, SeatsAvailable = 25 };
            var student1 = new Student { Id = 1, FirstName = "John", LastName = "Doe" };
            var student2 = new Student { Id = 2, FirstName = "Jane", LastName = "Doe" };
            var enrollments = new List<Enrollment>
            {
                new Enrollment { CourseId = 1, StudentId = 1, Progress = 0, IsCompleted = false, Course = course1, Student = student1 },
                new Enrollment { CourseId = 2, StudentId = 2, Progress = 50, IsCompleted = false, Course = course2, Student = student2 }
            };
            await _context.Courses.AddRangeAsync(course1, course2);
            await _context.Students.AddRangeAsync(student1, student2);
            await _context.Enrollments.AddRangeAsync(enrollments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _enrollmentRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public async Task GetByIdAsync_EnrollmentExists_ReturnsEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1, Progress = 0, IsCompleted = false };
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _enrollmentRepository.GetByIdAsync(1, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(1, result.StudentId);
        }

        [Test]
        public async Task GetByIdAsync_EnrollmentDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _enrollmentRepository.GetByIdAsync(1, 1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddAsync_AddsEnrollment_ReturnsEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1, Progress = 0, IsCompleted = false };

            // Act
            var result = await _enrollmentRepository.AddAsync(enrollment);
            var addedEnrollment = await _context.Enrollments.FindAsync(1, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(enrollment.CourseId, addedEnrollment.CourseId);
            Assert.AreEqual(enrollment.StudentId, addedEnrollment.StudentId);
        }

        [Test]
        public async Task UpdateAsync_UpdatesEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1, Progress = 0, IsCompleted = false };
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            enrollment.Progress = 50;

            // Act
            await _enrollmentRepository.UpdateAsync(enrollment);
            var updatedEnrollment = await _context.Enrollments.FindAsync(1, 1);

            // Assert
            Assert.AreEqual(50, updatedEnrollment.Progress);
        }

        [Test]
        public async Task DeleteAsync_EnrollmentExists_DeletesEnrollment()
        {
            // Arrange
            var enrollment = new Enrollment { CourseId = 1, StudentId = 1, Progress = 0, IsCompleted = false };
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            // Act
            await _enrollmentRepository.DeleteAsync(1, 1);
            var deletedEnrollment = await _context.Enrollments.FindAsync(1, 1);

            // Assert
            Assert.IsNull(deletedEnrollment);
        }

        [Test]
        public async Task DeleteAsync_EnrollmentDoesNotExist_NoOperation()
        {
            // Act
            await _enrollmentRepository.DeleteAsync(1, 1);

            // Assert
            var deletedEnrollment = await _context.Enrollments.FindAsync(1, 1);
            Assert.IsNull(deletedEnrollment);
        }
    }
}
