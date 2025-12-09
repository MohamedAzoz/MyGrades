namespace MyGrades.Application.Contracts.DTOs.Grade
{
    public class GradeExcelWriterDto
    {
        public string StudentName { get; set; }
        public double Attendance { get; set; }
        public double Tasks { get; set; }
        public double Practical { get; set; }
        public double TotalScore { get; set; }
    }
}
