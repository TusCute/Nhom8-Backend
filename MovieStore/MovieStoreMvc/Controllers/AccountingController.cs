using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Repositories.Abstract;

public class AccountingController : Controller
{   
    private readonly IUserAuthenticationService _userAuthService;

    public AccountingController(IUserAuthenticationService userAuthService)
    {
        _userAuthService = userAuthService;
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListAccount()
    {
        var users = await _userAuthService.GetAllUsersAsync();
        return View(users);
    }
}
