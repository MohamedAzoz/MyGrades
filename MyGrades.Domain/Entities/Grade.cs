namespace MyGrades.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public double Attendance { get; set; } 
        public double Tasks { get; set; }      
        public double Practical { get; set; } 
        public double TotalScore { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
