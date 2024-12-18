using CourseManagementSystem.API.Models;

namespace CourseManagementSystem.API.Factories.Interfaces
{
    public interface ICourseFactory 
    { 
        Course CreateCourse(string courseType); 
    }
}
