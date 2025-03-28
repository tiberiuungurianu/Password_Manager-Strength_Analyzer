using System.ComponentModel.DataAnnotations;

namespace draft1_cw2.Models
{
    public class VerifyCodeViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
