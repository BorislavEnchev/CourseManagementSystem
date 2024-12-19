using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>A list of students.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching students.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>The student with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { Message = "Student not found." });
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the student.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="student">The student to create.</param>
        /// <returns>The newly created student.</returns>
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                var newStudent = await _studentService.CreateStudentAsync(student);
                return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the student.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="id">The ID of the student to update.</param>
        /// <param name="student">The updated student information.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            try
            {
                if (id != student.Id)
                {
                    return BadRequest(new { Message = "Student ID mismatch." });
                }

                await _studentService.UpdateStudentAsync(student);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the student.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the student.", Details = ex.Message });
            }
        }
    }
}
