using BlazorUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorUi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class McapiController : ControllerBase
{
    private readonly ILogger<McapiController> _logger;

    public McapiController(ILogger<McapiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string symbol, double price, string account, string type, TraderWorker worker)
    {
        _logger.LogInformation($"{DateTime.Now} Symbol {symbol}, price {price}, account {account}, type {type}");

        if (type == "OPEN")
            await (worker.SignalOnOpenAsync(symbol, price, account);
        else if (type == "CLOSE")
            worker.SignalOnClose(symbol, price, account);
        return Ok();
    }
}
