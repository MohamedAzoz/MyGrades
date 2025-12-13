using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Projections_Models.Grades
{
    public class GradeModelData
    {
        public int Id { get; set; }
        public double Attendance { get; set; }
        public double Tasks { get; set; }
        public double Practical { get; set; }
        public double TotalScore { get; set; }
        public string StudentFullName { get; set; }
    }
    public class GradeModel
    {
        public Grade Grade { get; set; }
        public string SubjectName { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNationalId { get; set; }
    }
}
