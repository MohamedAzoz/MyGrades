namespace MyGrades.Application.Contracts.DTOs.User.Student
{
    public class StudentCreateModel
    {
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; } 
    }
}
