using BlazorUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorUi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class McapiController : ControllerBase
{
    private readonly ILogger<McapiController> _logger;
    private readonly TraderWorker _worker;

    public McapiController(ILogger<McapiController> logger, TraderWorker worker)
    {
        _logger = logger;
        _worker = worker;
    }

    [HttpGet]
    public IActionResult Get (string symbol, double price, string account, string type )
    {
        _logger.LogInformation($"{DateTime.Now} Symbol {symbol}, price {price}, account {account}, type {type}");

        //if (type == "OPEN")
        //    worker.SignalOnOpenAsync(symbol, price, account);
        //else if (type == "CLOSE")
        //    worker.SignalOnClose(symbol, price, account);
        return Ok();
    }
}
