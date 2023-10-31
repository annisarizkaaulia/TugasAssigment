using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class Employee : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
