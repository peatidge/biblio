using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Tela.Data.Core;
using Tela.Data.Organisation.Inventory;
using Tela.Models;
using Tela.Services;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 369. Add BookController to Administration Area 
public class BookController : AdministrationController
{
    private readonly ReferenceService _referenceService;
    public BookController(ApplicationDbContext context, ILogger<BookController> logger, ReferenceService catalogueService)
        : base(context, logger)
    {
        _referenceService = catalogueService;
    }
    //TODO:b 372. Add Index Action to BookController in Administration Area
    //id = ReferenceId
    public async Task<IActionResult> Index(int id, int pageNumber = 1, int pageSize = 5)
    => View(new PagedResult<BookDTO>
    {
        Data = (await _referenceService.GetBooksAsync<BookDTO>(LibraryId,Librarian.Id,id,TemporalisLibri.ActiveTransactionsAndRestorations, (pageSize * pageNumber) - pageSize, pageSize)).ToList(),
        TotalItems = await _referenceService.GetBookCountAsync(LibraryId, id),
        PageNumber = pageNumber,
        PageSize = pageSize
    });
    //TODO:b 375. Add Create Action to BookController
    [HttpPost]
    public async Task<IActionResult> Create(int id)
    {
        try
        {
            if (!Q.CoinFlip(65)) { 
                throw new DomainRuleException("Coffee Composition Exception");
            }
            var reference = await _context.References.FindAsync(id);
            if (reference == null) return NotFound();
            var b = reference.RegisterBook();
            await _context.SaveChangesAsync();
            TempData["Message"] = $"Book {b.UID} has been replicated";
            TempData["Alert"] = "success";
        }
        catch (DomainRuleException ex)
        {
            TempData["Message"] = Q.W;
            TempData["Alert"] = "danger";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            TempData["Message"] = Q.W;
            TempData["Alert"] = "danger";
        }
        return RedirectToAction(nameof(Index), new { id });    
    }
    //TODO:b 377. Add Details Action to BookController
    //id = UID
    public async Task<IActionResult> Details(Guid id) 
        => View(await _referenceService.GetBookAsync<BookDTO>(LibraryId, Librarian.Id,id,TemporalisLibri.ActiveTransactionsAndRestorations));
    //TODO:b 394. Add GET: Administration/Book/Search (searchTerm)
    [HttpGet]
    public async Task<IActionResult> Search(string searchTerm, State.Types? state = null)
    => Json((await _referenceService.GetBooksAsync<BookDTO>(LibraryId, searchTerm)).Where(b => !state.HasValue || b.State == state.Value));

}