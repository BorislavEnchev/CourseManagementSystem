namespace CourseManagementSystem.API.Models
{
    public class Enrollment { 
        public int Id { get; set; } 
        public int CourseId { get; set; } 
        public Course? Course { get; set; } 
        public int StudentId { get; set; } 
        public Student? Student { get; set; } 
        public DateTime EnrollmentDate { get; set; }
        public int Progress { get; set; } // To track the progress in percents
        public bool IsCompleted { get; set; } 
    }
}
