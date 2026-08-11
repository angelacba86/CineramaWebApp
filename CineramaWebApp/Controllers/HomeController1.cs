using Microsoft.AspNetCore.Mvc;

namespace CineramaWebApp.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
