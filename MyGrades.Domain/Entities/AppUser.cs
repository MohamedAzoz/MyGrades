using Microsoft.AspNetCore.Identity;

namespace MyGrades.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string NationalId { get; set; }  
        public string FullName { get; set; }    

    }
}
