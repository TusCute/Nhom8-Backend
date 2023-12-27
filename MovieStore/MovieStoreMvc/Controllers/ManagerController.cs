using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieStoreMvc.Controllers
{
    public class ManagerController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Manager()
        {
            return View("Manager");
        }
    }
}
