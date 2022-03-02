using Connectors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUi.Controllers
{
    public class InstrumentsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConnector _connector;

        public InstrumentsController(DataContext dataContext, IConnector connector)
        {
            _dataContext = dataContext;
            _connector = connector;
        }

        public async Task<IActionResult> Index()
        {
            var futs = await _dataContext.Futures.ToListAsync();
            return View(futs);
        }

        [HttpPost]
        public async Task<IActionResult> AddFuture(string localSymbol)
        {
            //var fut = new DbFuture();
            //if (_connector.RequestFuture(fut))
            //{
            //    if ((await _dataContext.Futures.FirstOrDefaultAsync(f => f.ConId == fut.ConId)) is null)
            //    {
            //        _dataContext.Futures.Add(fut);
            //        _dataContext.SaveChanges();
            //    }

            //}
            return Ok();
        }
    }
}
