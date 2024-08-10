using cloudscribe.Pagination.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tela.Areas.Administration.Models.Member;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Models;
using Tela.Services;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 292. Add MemberController to Administration Area (extend from Administration Controlller)
public class MemberController : AdministrationController
{
    //TODO:b 293. Add LibraryService to MemberController (constructor injection)
    private readonly LibraryService _libraryService; 
    public MemberController(ApplicationDbContext context, ILogger<MemberController> logger, LibraryService libraryService) 
        : base(context, logger)
    {
        _libraryService = libraryService;
    }
    /*TODO:b 298. Install Nuget Packages for Pagination
     * https://www.nuget.org/packages/cloudscribe.Web.Pagination
     * https://www.nuget.org/packages/cloudscribe.Pagination.Models
     */
    //TODO:b 303. Add GET: Administration/Member/Index
    //TODO:b 304 Add .table-responsive class to site.scss
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    => View(new PagedResult<Tela.Models.MemberDTO>
    {
        Data = (await _libraryService.GetMembersAsync<Tela.Models.MemberDTO>(LibraryId, (pageSize * pageNumber) - pageSize, pageSize)).ToList(),
        TotalItems = await _libraryService.GetMemberCountAsync(LibraryId),
        PageNumber = pageNumber,
        PageSize = pageSize
    });
    //TODO:b 295. Add GET: Administration/Member/Create 
    public IActionResult Create() => View();
    //TODO:b 296. Add POST: Administration/Member/Create (import Create model)
    [HttpPost]
    public async Task<IActionResult> Create(Create m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                //TODO:b 297. Run and verfiy the create action (check the database for a new user)
                await _libraryService.RegisterMemberAsync(LibraryId,m.Email!,m.FirstName!,m.LastName!);
                return RedirectToAction("Index");
            }
            catch (DomainRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                //TODO:b 319. Add Loxa as indication of error for end user if general exception occurs
                //The causality of the exception is logged for the developer
                ModelState.AddModelError("", Q.Loxa);
            }           
        }
        return View(m);
    }
    //TODO:b 393. Add GET: Administration/Member/Search (searchTerm)
    [HttpGet]
    public async Task<IActionResult> Search(string searchTerm)
    => Json(await _libraryService.GetMembersAsync<MemberDTO>(LibraryId,searchTerm));
  
}