using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// Retrieves all enrollments.
        /// </summary>
        /// <returns>A list of enrollments.</returns>
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

        /// <summary>
        /// Retrieves an enrollment by course ID and student ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The enrollment with the specified course ID and student ID.</returns>
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

        /// <summary>
        /// Enrolls a student in a course.
        /// </summary>
        /// <param name="enrollment">The enrollment details.</param>
        /// <returns>The newly created enrollment.</returns>
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

        /// <summary>
        /// Updates an existing enrollment.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="enrollment">The updated enrollment details.</param>
        /// <returns>No content if successful.</returns>
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

        /// <summary>
        /// Updates the progress of an enrollment.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="progress">The updated progress.</param>
        /// <returns>No content if successful.</returns>
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

        /// <summary>
        /// Deletes an enrollment by course ID and student ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>No content if successful.</returns>
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
