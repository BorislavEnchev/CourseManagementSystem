using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
            try
            {
                var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching enrollments.", Details = ex.Message });
            }
        }

        [HttpGet("{courseId}/{studentId}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int courseId, int studentId)
        {
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(courseId, studentId);
                if (enrollment == null)
                {
                    return NotFound(new { Message = "Enrollment not found." });
                }
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the enrollment.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> EnrollStudent(Enrollment enrollment)
        {
            try
            {
                var newEnrollment = await _enrollmentService.EnrollStudentAsync(enrollment);
                return CreatedAtAction(nameof(GetEnrollment),
                                       new { courseId = newEnrollment.CourseId, studentId = newEnrollment.StudentId },
                                       newEnrollment);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while enrolling the student.", Details = ex.Message });
            }
        }

        [HttpPut("{courseId}/{studentId}")]
        public async Task<IActionResult> UpdateEnrollment(int courseId, int studentId, Enrollment enrollment)
        {
            try
            {
                if (courseId != enrollment.CourseId || studentId != enrollment.StudentId)
                {
                    return BadRequest(new { Message = "Enrollment ID mismatch." });
                }

                await _enrollmentService.UpdateEnrollmentAsync(enrollment);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the enrollment.", Details = ex.Message });
            }
        }

        [HttpPut("{courseId}/{studentId}/progress")]
        public async Task<IActionResult> UpdateEnrollmentProgress(int courseId, int studentId, int progress)
        {
            try
            {
                await _enrollmentService.UpdateEnrollmentProgressAsync(courseId, studentId, progress);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating enrollment progress.", Details = ex.Message });
            }
        }

        [HttpDelete("{courseId}/{studentId}")]
        public async Task<IActionResult> DeleteEnrollment(int courseId, int studentId)
        {
            try
            {
                await _enrollmentService.DeleteEnrollmentAsync(courseId, studentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the enrollment.", Details = ex.Message });
            }
        }
    }
}
