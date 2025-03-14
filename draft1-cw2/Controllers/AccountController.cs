using Microsoft.AspNetCore.Mvc;

namespace YourMvcApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Replace this with your actual authentication logic.
            if (username == "admin" && password == "password")
            {
                // On successful login, redirect to Home page.
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }
    }
}