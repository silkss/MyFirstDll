using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers;

public class mcapiController : Controller
{
    private readonly DataContext _dataContext;

    public mcapiController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index(string symbol, float price, string signaltype)
    {
        var fut = _dataContext.Futures.FirstOrDefaultAsync(f => f.LocalSymbol == symbol);

        if (fut != null)
        {
            
        }
        return Ok();
    }
}
