using cloudscribe.Pagination.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Tela.Areas.Administration.Models.Reference;
using Tela.Data.Core;
using Tela.Data.Organisation.Inventory;
using Tela.Models;
using Tela.Services;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 347. Add ReferenceController to Administration area
public class ReferenceController : AdministrationController
{
    private readonly ReferenceService _referenceService;
    public ReferenceController(ApplicationDbContext context, ILogger<ReferenceController> logger, ReferenceService referenceService)
        : base(context, logger)
    {
        _referenceService = referenceService;
        //TODO:b 360. Mapster for Edit model is configured in constructor
        TypeAdapterConfig<Reference,Edit>.NewConfig().Ignore(d => d.Books);        
    }           
    //TODO:b 352. Add Index action to ReferenceController
    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5) => View(new PagedResult<ReferenceDTO>
    {
        Data = (await _referenceService.GetReferencesAsync<ReferenceDTO>(LibraryId,Librarian.Id,TemporalisLibri.Forgetful,(pageSize * pageNumber) - pageSize, pageSize)).ToList(),
        TotalItems = await _referenceService.GetReferenceCountAsync(LibraryId),
        PageNumber = pageNumber,
        PageSize = pageSize
    });
    //TODO:b 363. Add Details GET action to ReferenceController
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    => View(await _referenceService.GetReferenceAsync<ReferenceDTO>(LibraryId, Librarian.Id, id, TemporalisLibri.AllTransactionsAndRestorations)); 
    //TODO:b 349. Add Create GET/POST actions to ReferenceController
    //Also add IsUniqueISBN action for remote validation called from Create.cs
    [HttpGet]
    public IActionResult Create() => View(new Create { Books = 10 });
    [HttpPost]
    public async Task<IActionResult> Create(Create m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _referenceService.AddReferenceAsync(LibraryId, Librarian.Id, m.Title, m.ISBN, m.Author, m.Books);
                return RedirectToAction("Index");
            }
            catch (DomainRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ModelState.AddModelError("",Q.Loxa);
            }
        }
        return View(m);
    }
    [HttpGet]
    public async Task<IActionResult> IsUniqueISBN(string isbn)
    => await _referenceService.IsUniqueISBN(LibraryId, isbn) ? Json("ISBN already exists") : Json(true);
    //TODO:b 361. Add Edit GET/POST actions to ReferenceController
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var m = await _referenceService.GetReferenceAsync<Edit>(LibraryId, Librarian.Id, id, TemporalisLibri.Forgetful); 
        if (m == null)
        {
            return NotFound();
        }
        return View(m);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Edit m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _referenceService.UpdateReferenceAsync(LibraryId, Librarian.Id, m.Title, m.ISBN, m.Author);
                return RedirectToAction(nameof(this.Index));
            }
            catch (DomainRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ModelState.AddModelError("",Q.Loxa);
            }
        }
        return View(m);
    }
}