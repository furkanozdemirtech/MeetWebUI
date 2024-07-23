using Microsoft.AspNetCore.Identity;

namespace CreatedMeetWebUI.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Password { get; set; }
    }
}
