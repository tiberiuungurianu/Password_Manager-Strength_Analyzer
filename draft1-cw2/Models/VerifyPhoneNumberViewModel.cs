using System.ComponentModel.DataAnnotations;

namespace draft1_cw2.Models
{
    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}
