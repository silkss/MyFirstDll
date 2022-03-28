using Microsoft.AspNetCore.Mvc;

namespace BlazorUi.Controllers;

[ApiController]
[Route("[controller]")]
public class McapiController : ControllerBase
{
    private readonly ILogger<McapiController> _logger;

    public McapiController(ILogger<McapiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation($"{DateTime.Now} Get request");
        return Ok();
    }
}
