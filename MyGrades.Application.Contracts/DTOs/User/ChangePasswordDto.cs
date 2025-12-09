namespace MyGrades.Application.Contracts.DTOs.User
{
    public class ChangePasswordDto
    {
        public string NationalId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
