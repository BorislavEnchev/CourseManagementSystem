using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <returns>A list of courses.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching courses.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a course by ID.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return NotFound(new { Message = "Course not found." });
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the course.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="courseType">The type of the course to create.</param>
        /// <returns>The newly created course.</returns>
        [HttpPost("{courseType}")]
        public async Task<ActionResult<Course>> CreateCourse(string courseType)
        {
            try
            {
                var newCourse = await _courseService.CreateCourseAsync(courseType);
                return CreatedAtAction(nameof(GetCourse), new { id = newCourse.Id }, newCourse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the course.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="id">The ID of the course to update.</param>
        /// <param name="course">The updated course information.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            try
            {
                if (id != course.Id)
                {
                    return BadRequest(new { Message = "Course ID mismatch." });
                }

                await _courseService.UpdateCourseAsync(course);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the course.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a course by ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the course.", Details = ex.Message });
            }
        }
    }
}
