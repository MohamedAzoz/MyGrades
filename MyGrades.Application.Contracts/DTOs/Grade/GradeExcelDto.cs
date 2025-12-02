namespace MyGrades.Application.Contracts.DTOs.Grade
{
    public class GradeExcelDto
    {
        public int StudentId { get; set; }
        public double Attendance { get; set; } // غياب
        public double Tasks { get; set; }      // تاسكات
        public double Practical { get; set; }
    }
}
