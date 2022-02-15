using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
