﻿using Connectors.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers;

public class HomeController : Controller
{
    private readonly IConnector _Connector;
    public HomeController(IConnector connector)
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
