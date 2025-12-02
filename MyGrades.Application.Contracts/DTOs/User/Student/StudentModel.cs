namespace MyGrades.Application.Contracts.DTOs.User.Student
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public int AcademicYearId { get; set; }
        public int DepartmentId { get; set; }
        //public string? EmailAddress { get; set; }

    }
}
