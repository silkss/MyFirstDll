using Connectors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUi.Controllers
{
    public class InstrumentsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConnector<DbFuture, DbOption> _connector;

        public InstrumentsController(DbWorker dbWorker, DataContext dataContext, IConnector<DbFuture, DbOption> connector)
        {
            _dataContext = dataContext;
            _connector = connector;
        }

        public async Task<IActionResult> Index()
        {
            var fut = await _dataContext.Futures.ToListAsync();
            ViewData["NumberOfFutures"] = fut.Count;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFuture(string localSymbol)
        {
            var fut = await _connector.RequestFutureAsync(localSymbol);
            if (fut != null)
            {
                if ( (await _dataContext.Futures.FirstOrDefaultAsync(f => f.ConId == fut.ConId)) is null)
                {
                    _dataContext.Futures.Add(fut);
                    _dataContext.SaveChanges();
                }
            }
            return Ok();
        }
    }
}
