namespace MyGrades.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } // FK to ApplicationUser
        public AppUser? User { get; set; }

        public ICollection<Department>? Departments { get; set; }
        public ICollection<Subject>? Subjects { get; set; }

    }
}
