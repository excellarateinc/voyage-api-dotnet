using System.ComponentModel.DataAnnotations;

namespace Launchpad.Models
{
    public class RegistrationModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        [Required]
        [StringLength(128, MinimumLength = 1)]

        public string LastName { get; set; }
    }
}
