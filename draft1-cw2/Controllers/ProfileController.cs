using draft1_cw2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace draft1_cw2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Profile/Settings
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserProfileViewModel[Authorize]
public class ProfileController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<IActionResult> Index()
            {
                var user = await _userManager.GetUserAsync(User);
                var model = new UserProfileViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Preferences = user.Preferences,
                    IsMfaEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
                };
                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.FullName = model.FullName;
                user.Preferences = model.Preferences;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Profile updated successfully!";
                }
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<IActionResult> ChangePassword(UserProfileViewModel model)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["Success"] = "Password changed successfully!";
                }
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<IActionResult> ToggleMfa(bool enable)
            {
                var user = await _userManager.GetUserAsync(User);
                await _userManager.SetTwoFactorEnabledAsync(user, enable);
                TempData["Success"] = enable ? "MFA enabled!" : "MFA disabled!";
                return RedirectToAction("Index");
            }
        }

    }
