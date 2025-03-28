using Microsoft.AspNetCore.Identity;

namespace draft1_cw2.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Extra profile fields
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // New Preferences property to store user settings or preferences
        public string Preferences { get; set; }
    }
}
