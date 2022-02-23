using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers
{
    public class ContainersController : Controller
    {
        public IActionResult Index(StrategeisRepository strategeisRepository)
        {
            return View();
        }
    }
}
