using CourseManagementSystem.API.Data;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace OnlineCourseManagementSystem.Tests.Repositories
{
    [TestFixture]
    public class CourseRepositoryTests
    {
        private ApplicationDbContext _context;
        private CourseRepository _courseRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _courseRepository = new CourseRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 },
                new Course { Id = 2, Title = "Course 2", Description = "Description 2", Credits = 4, SeatsAvailable = 25 }
            };
            await _context.Courses.AddRangeAsync(courses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _courseRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_CourseExists_ReturnsCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 };
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            // Act
            var result = await _courseRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_CourseDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _courseRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddAsync_AddsCourse_ReturnsCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 };

            // Act
            var result = await _courseRepository.AddAsync(course);
            var addedCourse = await _context.Courses.FindAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(course.Title, addedCourse.Title);
        }

        [Test]
        public async Task UpdateAsync_UpdatesCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 };
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            course.Title = "Updated Course 1";

            // Act
            await _courseRepository.UpdateAsync(course);
            var updatedCourse = await _context.Courses.FindAsync(1);

            // Assert
            Assert.AreEqual("Updated Course 1", updatedCourse.Title);
        }

        [Test]
        public async Task DeleteAsync_CourseExists_DeletesCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Course 1", Description = "Description 1", Credits = 3, SeatsAvailable = 30 };
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            // Act
            await _courseRepository.DeleteAsync(1);
            var deletedCourse = await _context.Courses.FindAsync(1);

            // Assert
            Assert.IsNull(deletedCourse);
        }

        [Test]
        public async Task DeleteAsync_CourseDoesNotExist_NoOperation()
        {
            // Act
            await _courseRepository.DeleteAsync(1);

            // Assert
            var deletedCourse = await _context.Courses.FindAsync(1);
            Assert.IsNull(deletedCourse);
        }
    }
}
