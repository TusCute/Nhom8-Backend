using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;

namespace MovieStoreMvc.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        private readonly UserManager<ApplicationUser> userManager;  // Inject UserManager<ApplicationUser>

        public UserAuthenticationController(IUserAuthenticationService authService, UserManager<ApplicationUser> userManager)
        {
            this.authService = authService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.RegisterAsync(model);

                if (result.StatusCode == 1)
                {
                    TempData["msg"] = result.Message;
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = result.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                var user = await userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    if (userRoles.Contains("Admin"))
                    {
                        return RedirectToAction("MovieList", "Movie");
                    }
                    else
                    {
                        TempData["msg"] = "You do not have permission to access this page.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["msg"] = "User not found.";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["msg"] = "Could not log in.";
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
