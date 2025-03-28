

    using System.ComponentModel.DataAnnotations;

    namespace draft1_cw2.Models
    {
        public class LoginViewModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }

