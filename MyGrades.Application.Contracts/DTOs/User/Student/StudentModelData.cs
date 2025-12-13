namespace MyGrades.Application.Contracts.DTOs.User.Student
{
    public class StudentModelData
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public string AcademicLevel { get; set; }
        public string Department { get; set; } 
    }
}
