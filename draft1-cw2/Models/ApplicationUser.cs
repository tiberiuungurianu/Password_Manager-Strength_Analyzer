using Microsoft.AspNetCore.Identity;

namespace draft1_cw2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } // Additional custom properties
    }
}
