namespace MyGrades.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Student>? Students { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }
        public ICollection<Assistant>? Assistants { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
    }
}
