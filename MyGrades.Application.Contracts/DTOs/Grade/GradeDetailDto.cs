namespace MyGrades.Application.Contracts.DTOs.Grade
{
    public class GradeDetailDto
    {
        public string CourseName { get; set; }
        public double Attendance { get; set; }
        public double Tasks { get; set; }
        public double Practical { get; set; }
        public double TotalScore { get; set; }
    }
}
