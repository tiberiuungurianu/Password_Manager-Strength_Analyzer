using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using draft1_cw2.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace draft1_cw2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Constructor: inject Identity services
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // -------------------------------------------
        // LOGIN
        // -------------------------------------------

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Attempt to sign in the user with username and password.
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else if (result.RequiresTwoFactor)
            {
                // Redirect to the two-factor authentication step.
                return RedirectToAction("VerifyCode", new { returnUrl, rememberMe = model.RememberMe });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        // Verify Code 

        [HttpGet]
        public async Task<IActionResult> VerifyCode(string returnUrl = null, bool rememberMe = false)
        {
            // Ensure the user has already passed the username/password step.
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // If returnUrl is null or empty, set a default value.
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            // Generate a random 6-digit demo code.
            var random = new Random();
            var demoCode = random.Next(100000, 1000000).ToString();

            // Store the demo code in TempData.
            TempData["DemoCode"] = demoCode;

            var model = new VerifyCodeViewModel
            {
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };
            return View(model);
        }


        // POST: /Account/VerifyCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve the demo code using Peek so it remains in TempData.
            var demoCode = TempData.Peek("DemoCode") as string;
            if (string.IsNullOrEmpty(demoCode))
            {
                ModelState.AddModelError("", "The demo code has expired. Please log in again.");
                return View(model);
            }

            Console.WriteLine("Demo code from TempData: " + demoCode);
            Console.WriteLine("Code entered by user: " + model.Code);

            if (model.Code == demoCode)
            {
                // Retrieve the two-factor authentication user.
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    ModelState.AddModelError("", "Unable to retrieve two-factor authentication user.");
                    return View(model);
                }

                // Code is correct, manually sign in the user.
                await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                Console.WriteLine("User signed in successfully.");

                return RedirectToLocal(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid code.");
                return View(model);
            }
        }


        // -------------------------------------------
        // REGISTER
        // -------------------------------------------

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Create a new user from the data in RegisterViewModel
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName, // If you have these fields in ApplicationUser
                LastName = model.LastName
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Optionally, add the user to a default role:
                // await _userManager.AddToRoleAsync(user, "User");

                // Automatically sign in the user (optional)
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to home (or wherever you want)
                return RedirectToAction("Index", "Home");
            }

            // If registration failed, add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // -------------------------------------------
        // LOGOUT
        // -------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // -------------------------------------------
        // HELPER: RedirectToLocal
        // -------------------------------------------
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            // Populate the view model from the user
            var model = new UserProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsMfaEnabled = user.TwoFactorEnabled,
                IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
            };

            return View(model);
        }
        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Update the user properties
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;   
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.StatusMessage = "Your profile has been updated.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // POST: /Account/DisableMfa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableMfa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Disable Two-Factor Authentication
            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "Multi-Factor Authentication has been disabled.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("Profile");
        }


        // GET: /Account/AddPhoneNumber
        [HttpGet]
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Account/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Generate the token for phone number verification.
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);

            // Send the SMS using an SMS provider.
            // For demonstration purposes, you might log or display the code.
            // e.g., await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            // For now, we use TempData to pass the code to the verification page (not for production use).
            TempData["DemoCode"] = code;

            // Redirect to verification step.
            return RedirectToAction("VerifyPhoneNumber", new { phoneNumber = model.PhoneNumber });
        }

        // GET: /Account/VerifyPhoneNumber
        [HttpGet]
        public IActionResult VerifyPhoneNumber(string phoneNumber)
        {
            var model = new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber };
            return View(model);
        }

        // POST: /Account/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Attempt to change (verify) the phone number using the provided code
            var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                // Enable Two-Factor Authentication after successful phone verification
                var setMfaResult = await _userManager.SetTwoFactorEnabledAsync(user, true);
                if (setMfaResult.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["StatusMessage"] = "Your phone number has been verified and MFA is enabled.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in setMfaResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                // If ChangePhoneNumberAsync fails, show the specific error messages
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If we reach here, something failed, so redisplay the form
            return View(model);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.StatusMessage = "Your password has been changed.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }


    }
}