namespace MyGrades.Domain.Entities
{
    public class AcademicLevel
    {
        public int Id { get; set; }
        public string LevelName { get; set; }

        public ICollection<Student>? Students { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
    }
}
