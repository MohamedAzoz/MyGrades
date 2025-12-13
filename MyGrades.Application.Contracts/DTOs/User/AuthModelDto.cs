
namespace MyGrades.Application.Contracts.DTOs.User
{
    public class AuthModelDto
    {
        public string Message { get; set; }
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; }
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
