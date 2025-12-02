namespace MyGrades.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int AssistantId { get; set; }
        public Assistant? Assistant { get; set; }

        public int AcademicLevelId { get; set; }
        public AcademicLevel? AcademicLevel { get; set; }
        public ICollection<Grade>? Grades { get; set; }
    }
}
