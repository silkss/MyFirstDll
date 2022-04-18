using Connectors.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MvcUi.Controllers;

public class ConnectorController : Controller
{
    private readonly IConnector _connector;

    public ConnectorController(IConnector connector)
    {
        _connector = connector;
    }
    public IActionResult Index()
    {

        ViewData["connected"] = _connector.IsConnected ? "connected" : "not connected";
        return View();
    }
    public IActionResult Connect()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Connect(string ip, int port, int clientid)
    {
        _connector.Connect(ip, port, clientid);
        return RedirectToAction("Index");
    }

    public IActionResult Disconnect()
    {
        _connector.Disconnect();
        return RedirectToAction("Index");
    }
}
