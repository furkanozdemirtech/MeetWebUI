using System.ComponentModel.DataAnnotations;

namespace CreatedMeetWebUI.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least{2} and at max {1} characters log.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Comfirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ComfirmPassword { get; set; }
    }
}
