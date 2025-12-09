namespace MyGrades.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } 
        public AppUser? User { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int AcademicLevelId { get; set; }
        public AcademicLevel? AcademicLevel { get; set; }

        public ICollection<StudentSubject>? StudentSubjects { get; set; }
        //public ICollection<Grade>? Grades { get; set; }
    }
}
