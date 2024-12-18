using CourseManagementSystem.API.Models;

namespace CourseManagementSystem.API.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment?> GetEnrollmentByIdAsync(int courseId, int studentId);
        Task<Enrollment> EnrollStudentAsync(Enrollment enrollment);
        Task UpdateEnrollmentProgressAsync(int courseId, int studentId, int progress);
        Task UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(int courseId, int studentId);
    }

}
