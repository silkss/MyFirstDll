using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers
{
    public class mcapiController : Controller
    {
        public IActionResult Index(string symbol, float price, string signaltype)
        {
            return Ok();
        }
    }
}
