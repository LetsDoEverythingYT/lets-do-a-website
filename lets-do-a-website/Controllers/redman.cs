using Microsoft.AspNetCore.Mvc;

namespace lets_do_a_website.Controllers
{
    public class redman : Controller
    {
        public IActionResult Index(int id=1)
        {
            ViewData["bookId"] = id;
            return View();
        }
    }
}
