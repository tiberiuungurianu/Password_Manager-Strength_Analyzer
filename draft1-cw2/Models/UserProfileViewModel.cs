namespace draft1_cw2.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Preferences { get; set; }

        public bool IsMfaEnabled { get; set; }
        public bool IsAdmin { get; set; }

        // For password change
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
