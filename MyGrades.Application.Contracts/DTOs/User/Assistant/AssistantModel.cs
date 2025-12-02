namespace MyGrades.Application.Contracts.DTOs.User.Assistant
{
    public class AssistantModel
    {
        public int Id { get; set; }
        //public string UserId { get; set; }
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
    }
}
