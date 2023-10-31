using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
