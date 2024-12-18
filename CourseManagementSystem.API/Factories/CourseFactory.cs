using CourseManagementSystem.API.Factories.Interfaces;
using CourseManagementSystem.API.Models;

namespace OnlineCourseManagementSystem.API.Factories
{
    public class CourseFactory : ICourseFactory
    {
        public Course CreateCourse(string courseType)
        {
            return courseType switch
            {
                "Online" => new Course
                {
                    Title = "Online Course Title",
                    Description = "Description for Online Course",
                    Credits = 3,
                    SeatsAvailable = int.MaxValue // Assuming online courses have unlimited seats
                },
                "Offline" => new Course
                {
                    Title = "Offline Course Title",
                    Description = "Description for Offline Course",
                    Credits = 3,
                    SeatsAvailable = 30 // Assuming a default seat capacity for offline courses
                },
                _ => throw new ArgumentException("Invalid course type", nameof(courseType)),
            };
        }
    }
}
