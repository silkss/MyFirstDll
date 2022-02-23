using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers
{
    public class ContainersController : Controller
    {
        public IActionResult Index(StrategeisRepository strategeisRepository)
        {
            var containers = strategeisRepository.GetAll();
            return View(containers);
        }
    }
}
