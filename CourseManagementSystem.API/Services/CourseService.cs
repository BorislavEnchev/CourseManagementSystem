using CourseManagementSystem.API.Factories.Interfaces;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Services.Interfaces;

namespace CourseManagementSystem.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseFactory _courseFactory;

        public CourseService(ICourseRepository courseRepository, ICourseFactory courseFactory)
        {
            _courseRepository = courseRepository;
            _courseFactory = courseFactory;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<Course> CreateCourseAsync(string courseType)
        {
            var course = _courseFactory.CreateCourse(courseType);
            return await _courseRepository.AddAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteAsync(id);
        }
    }

}
