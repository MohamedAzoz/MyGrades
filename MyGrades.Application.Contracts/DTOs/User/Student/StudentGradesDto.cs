using MyGrades.Application.Contracts.DTOs.Grade;

namespace MyGrades.Application.Contracts.DTOs.User.Student
{
    public class StudentGradesDto
    {
        public string StudentName { get; set; }
        public List<GradeDetailDto> Grades { get; set; }
    }
}
