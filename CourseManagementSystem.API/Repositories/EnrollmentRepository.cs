using CourseManagementSystem.API.Data;
using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.API.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                                 .Include(e => e.Course)
                                 .Include(e => e.Student)
                                 .ToListAsync();
        }

        public async Task<Enrollment?> GetByIdAsync(int courseId, int studentId)
        {
            return await _context.Enrollments
                                 .FirstOrDefaultAsync(e => e.CourseId == courseId && 
                                                           e.StudentId == studentId);
        }

        public async Task<Enrollment> AddAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int courseId, int studentId)
        {
            var enrollment = await _context.Enrollments
                                           .FirstOrDefaultAsync(e => e.CourseId == courseId && 
                                                                     e.StudentId == studentId);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }
    }

}
