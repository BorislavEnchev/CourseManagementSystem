namespace CourseManagementSystem.API.Models
{
    public class Course { 
        public int Id { get; set; } 
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public int Credits { get; set; } 
        public int SeatsAvailable { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; } 
    }
}
