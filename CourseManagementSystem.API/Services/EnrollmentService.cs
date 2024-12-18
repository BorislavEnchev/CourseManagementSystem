using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Services.Interfaces;

namespace CourseManagementSystem.API.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _enrollmentRepository.GetAllAsync();
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int courseId, int studentId)
        {
            return await _enrollmentRepository.GetByIdAsync(courseId, studentId);
        }

        public async Task<Enrollment> EnrollStudentAsync(Enrollment enrollment)
        {
            var newEnrollment = await _enrollmentRepository.AddAsync(enrollment);
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            if (course != null)
            { 
                course.SeatsAvailable -= 1;
                await _courseRepository.UpdateAsync(course);
            }
            return newEnrollment;
        }

        public async Task UpdateEnrollmentProgressAsync(int courseId, int studentId, int progress) 
        { 
            var enrollment = await _enrollmentRepository.GetByIdAsync(courseId, studentId); 
            if (enrollment != null) 
            { 
                enrollment.Progress = progress; 
                if (progress >= 100) 
                { 
                    enrollment.IsCompleted = true; 
                } 
                await _enrollmentRepository.UpdateAsync(enrollment); 
            } 
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            await _enrollmentRepository.UpdateAsync(enrollment);
        }

        public async Task DeleteEnrollmentAsync(int courseId, int studentId)
        {
            await _enrollmentRepository.DeleteAsync(courseId, studentId);
        }
    }

}
