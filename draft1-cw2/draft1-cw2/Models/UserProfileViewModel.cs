using System.ComponentModel.DataAnnotations;

namespace draft1_cw2.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Phone] // validates phone number format
        public string PhoneNumber { get; set; }

        [Display(Name = "MFA Enabled")]
        public bool IsMfaEnabled { get; set; }

        // This property is used to conditionally display admin-only settings.
        public bool IsAdmin { get; set; }
    }
}
