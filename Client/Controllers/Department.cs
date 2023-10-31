using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class Department : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
