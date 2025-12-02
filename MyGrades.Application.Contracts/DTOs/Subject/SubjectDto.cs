namespace MyGrades.Application.Contracts.DTOs.Subject
{
    public class SubjectDto
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int AssistantId { get; set; }
        public int AcademicLevelId { get; set; }
    }
}
