using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Tela.Areas.Administration.Models.Restoration;
using Tela.Data.Core;
using Tela.Models;
using Tela.Services;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 414. Add RestorationController to Administration
public class RestorationController : AdministrationController
{
    private readonly RestorationService _restorationService;
    public RestorationController(ApplicationDbContext context, ILogger<RestorationController> logger, RestorationService restorationService )
        : base(context, logger) 
    {
        _restorationService = restorationService;   
    }
    //TODO:b 415. Add Index method to RestorationController
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
   => View(new PagedResult<RestorationDTO>
   {
       Data = (await _restorationService.GetRestorationsAsync<RestorationDTO>(LibraryId,Librarian.Id,TemporalisLibri.AllRestorations,(pageSize * pageNumber) - pageSize, pageSize)).ToList(),
       TotalItems = await _restorationService.GetRestorationCountAsync(LibraryId,Librarian.Id, TemporalisLibri.AllRestorations),
       PageNumber = pageNumber,
       PageSize = pageSize
   });
    //TODO:b 421. Add administration-restoration-schedule.js
    //TODO:b 422. Add Schedule GET/POST methods to RestorationController
    [HttpGet]
    public async Task<IActionResult> Schedule(Guid? uid)
    => View(new Schedule
    {
        Start = DateTime.Now.AddDays(7),
        End = DateTime.Now.AddDays(14),
        Book = uid.HasValue ? await _restorationService.GetBookAsync<BookDTO>(LibraryId,uid.Value) : default!
    });
    [HttpPost]
    public async Task<IActionResult> Schedule(Schedule m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _restorationService.ScheduleRestorationAsync(LibraryId, Librarian, m.Book.UID!.Value, m.Start!.Value, m.End!.Value);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ModelState.AddModelError("", Q.Loxa);
            }
        }
        return View(m);
    }
    //TODO:b 420. Add DateIsInFuture method to RestorationController
    [AcceptVerbs("GET", "POST")]
    public IActionResult DateIsInFuture(DateTime? start, DateTime? end)
    => Json((start.IsAndIsNotFuturistic() || end.IsAndIsNotFuturistic())
        ? $"{(start.IsAndIsNotFuturistic() ? "Start" : "End")} must be in this particular dimension's future." : true
    );
}