using CourseManagementSystem.API.Models;

namespace CourseManagementSystem.API.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course> CreateCourseAsync(string courseType);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
    }

}
