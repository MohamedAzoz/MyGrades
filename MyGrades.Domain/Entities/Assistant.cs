namespace MyGrades.Domain.Entities
{
    public class Assistant 
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } // FK to ApplicationUser
        public AppUser? User { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Subject>? Subjects { get; set; }

    }
}
