using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
