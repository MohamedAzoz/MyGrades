using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Projections_Models.Grades
{
    public class GradeModel
    {
        public Grade Grade { get; set; }
        public string SubjectName { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNationalId { get; set; }
    }
}
