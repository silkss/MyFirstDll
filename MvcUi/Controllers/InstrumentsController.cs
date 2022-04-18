using Connectors.Interfaces;
using DataLayer.Models.Instruments;
using Microsoft.AspNetCore.Mvc;
using MvcUi.Services.Repositories;

namespace MvcUi.Controllers;

public class InstrumentsController : Controller
{
    private readonly FutureRepository _futureRepository;
    private readonly IConnector _connector;

    public InstrumentsController(FutureRepository futureRepository, IConnector connector)
    {
        _futureRepository = futureRepository;
        _connector = connector;
    }

    public IActionResult Index()
    {
        var objInstrumentsList = _futureRepository.GetAll();
        return View(objInstrumentsList);
    }
    public IActionResult Info(int? id)
    {
        if (!id.HasValue) return NotFound();
        
        var obj = _futureRepository.GetById(id.Value);
        
        if (obj == null) return NotFound();

        return View(obj);
    }
    public IActionResult Delete(int? id)
    {
        if (!id.HasValue) return NotFound();

        _futureRepository.DeleteById(id.Value);

        return RedirectToAction("Index");
    }
    public IActionResult Add(string localSymbol, string exchange)
    {
        if (string.IsNullOrEmpty(localSymbol) || string.IsNullOrEmpty(exchange)) return NotFound();

        var future = new DbFuture
        {
            LocalSymbol = localSymbol.Trim().ToUpper(),
            Echange = exchange.Trim().ToUpper(),
        };
        if (!_connector.IsConnected) return RedirectToAction("Connect", "Connector");
        if (_connector.TryRequestFuture(future, future.Echange))
        {
            _futureRepository.Add(future);
            return RedirectToAction("Index");
        }
        return NotFound();
    }
}
