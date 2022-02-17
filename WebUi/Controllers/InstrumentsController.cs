using Connectors.Interfaces;
using DataLayer.Models.Instruments;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUi.Controllers
{
    public class InstrumentsController : Controller
    {

        private readonly DataContext _dbcontext;
        private readonly IConnector<DbFuture, DbOption> _connector;

        public InstrumentsController(DataContext dbcontext, IConnector<DbFuture,DbOption> connector)
        {
            _dbcontext = dbcontext;
            _connector = connector;
        }
        public async Task<IActionResult> Index()
        {
            var futs = await _dbcontext.Futures.ToListAsync();
            var options = await _dbcontext.Options.ToListAsync();

            ViewData["NumberOfFutures"] = futs.Count;
            ViewData["NumberOfOptions"] = options.Count;
            return View();
        }

        [HttpPost]
        public Task<IActionResult> AddFuture(string localSymbol)
        {
            _connector.RequestFuture(localSymbol);
            return Index();
        }
        public async Task<IActionResult> Futures()
        {
            var futs = await _dbcontext.Futures.ToListAsync();
            ViewData["Futures"] = futs;
            return View();
        }

        public async Task<IActionResult> Options()
        {
            var options = await _dbcontext.Options.ToListAsync();
            ViewData["Options"] = options;
            return View();
        }
    }
}
