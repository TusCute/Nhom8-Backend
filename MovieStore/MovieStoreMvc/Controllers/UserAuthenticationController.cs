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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserAuthenticationController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager, IUserAuthenticationService authService)
        {
            this.authService = authService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Name = model.Name };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Nếu người dùng đã chọn vai trò là "Admin", thêm người dùng vào vai trò "Admin"
                    if (model.Role == "Admin")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }

                    // Đăng nhập người dùng sau khi đăng ký
                    await signInManager.SignInAsync(user, isPersistent: false);

                    // Chuyển hướng đến trang chính
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is not valid, return back to the registration page with validation errors
            return View(model);
        }


        /* We will create a user with admin rights, after that we are going
          to comment this method because we need only
          one user in this application 
          If you need other users ,you can implement this registration method with view
          I have create a complete tutorial for this, you can check the link in description box
         */

        //public async Task<IActionResult> Register()
        //{
        //    var model = new RegistrationModel
        //    {
        //        Email = "admin@gmail.com",
        //        Username = "admin",
        //        Name = "Ravindra",
        //        Password = "Admin@123",
        //        PasswordConfirm = "Admin@123",
        //        Role = "Admin"
        //    };
        //    // if you want to register with user , Change Role="User"
        //    var result = await authService.RegisterAsync(model);
        //    return Ok(result.Message);
        //}


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
                return RedirectToAction("MovieList", "Movie");
            else
            {
                TempData["msg"] = "Could not logged in..";
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
