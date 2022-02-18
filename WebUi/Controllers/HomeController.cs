using Connectors.Interfaces;
using DataLayer.Models.Instruments;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers;

public class HomeController : Controller
{
    private readonly IConnector<DbFuture, DbOption> _Connector;
    public HomeController(IConnector<DbFuture, DbOption> connector)
    {
        _Connector = connector;
    }
    public IActionResult Index()
    {
        ViewData["Connected"] = _Connector.IsConnected;
        return View();
    }

    public IActionResult Connect()
    {
        if (!_Connector.IsConnected)
            _Connector.Connect();

        return View();
    }
}
