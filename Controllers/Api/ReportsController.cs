using Aspose.Html.Dom.Css;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Data.Records;
namespace Tela.Controllers.Api;
//TODO:b 379. Add ReportsController to Api
[ApiController, Route("api/[controller]"),Authorize(Roles = nameof(Roles.LIBRARIAN))]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    protected Librarian Librarian => _context.Users.OfType<Librarian>().First(l => l.UserName == User.Identity!.Name);
    protected int LibraryId => Librarian.LibraryId;
    public ReportsController(ApplicationDbContext context) => _context = context;
    //TODO:b 380. Add Tau Reports Api Controller
    [HttpGet, Route("books/tau/{uid}")]
    public IActionResult Tau(Guid uid,[FromQuery]DateTime start,[FromQuery]DateTime end) 
        => Ok(_context.Tau(uid,start,end));
    //TODO:b 428b. Add Book Status  endpoint to Reports Api Controller
    [HttpGet, Route("books/status")]
    public async Task<IActionResult> BookStatus()
     => Ok(await _context.LibraryBookStatuses.Where(lbs => lbs.Id == LibraryId).ToArrayAsync());
    //TODO:b 438. Add gravitons endpoint to Reports Api Controller
    [HttpGet, Route("gravitons/{o}"),AllowAnonymous]
    public async Task<IActionResult> Gravitons(int o)
    {
        var m = !(await _context.Set<Graviton>().AnyAsync(g => g.Order > o))
            ? await _context.Set<Graviton>().OrderBy(g => g.Order)
                .Select(g => new { g.Id, g.Order, g.Wave, g.Duration, Clocked = true })
                .FirstOrDefaultAsync()
            : await _context.Set<Graviton>().OrderBy(g => g.Order)
                .Where(g => g.Order > o)
                .Select(g => new { g.Id, g.Order, g.Wave, g.Duration, Clocked = false })
                .FirstOrDefaultAsync();
        return m != null ? Ok(m) : NotFound();
    }
    //TODO:b 472. Add Transactions Stat endpoint to Reports Api Controller
    [HttpGet, Route("transactions/stat")]
    public async Task<IActionResult> TransactionsStat(DateTime? start)
        => Ok(await _context.Database.SqlQueryRaw<TranceActionsStat>("GetTranceActionsStatim @libraryId, @start",
            new SqlParameter("@libraryId", LibraryId),
            new SqlParameter("@start", start.HasValue ? start : DateTime.Now.AddYears(-1))
        ).ToArrayAsync()); 
}