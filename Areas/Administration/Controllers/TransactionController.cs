using cloudscribe.Pagination.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Tela.Areas.Administration.Models.Transaction;
using Tela.Data.Core;
using Tela.Models;
using Tela.Services;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 385. Add TransactionController to Administration Area
public class TransactionController : AdministrationController
{
    private readonly TransactionService _transactionService;
    public TransactionController(ApplicationDbContext context, ILogger<TransactionController> logger, TransactionService transactionService )
        : base(context, logger)
    {
        _transactionService = transactionService;
    }
    //TODO:b 386. Add Index Action to TransactionController in Administration Area
    //TODO:b 480. Update Index to include start date filter
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, DateTime start = default)
    {
        start = start != DateTime.MinValue ? start : DateTime.Now.AddMonths(-3);
        ViewBag.Start = start.ToString("yyyy-MM-dd");
        int skip = (pageSize * pageNumber) - pageSize;
        var data = (await _transactionService.GetTransactionsAsync<TransactionDTO>(LibraryId, Librarian.Id, TemporalisLibri.AllTransactions, skip, pageSize, start: start)).ToList();
        int count = await _transactionService.GetTransactionCountAsync(LibraryId,start); 
        return View(new PagedResult<TransactionDTO>{ Data = data,TotalItems = count, PageNumber = pageNumber, PageSize = pageSize });
    } 
    //TODO:b 397. Add GET/POST: Administration/Transaction/Hold
    //TODO:b 398. Add search.js
    //TODO:b 399. Add administration-transaction-create.js 
    //TODO:b 400. Add .search-result & .input-container-readonly to site.scss
    [HttpGet]
    public async Task<IActionResult> Hold(Guid? uid = null, string? memberId = null)
    {
        var m = new Create
        {
            Member = memberId != null ? await _transactionService.GetMemberAsync<MemberDTO>(LibraryId,memberId) : default!,
            Book = uid.HasValue ? await _transactionService.GetBookAsync<BookDTO>(LibraryId, uid.Value) : default!,
            Type = Data.Organisation.Inventory.Transaction.TransactionType.Hold
        };
        return View("Create", m);
    }
    [HttpPost]
    public async Task<IActionResult> Hold(Create m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _transactionService.HoldBookAsync(LibraryId, Librarian.Id, m.Member!.Id!, m.Book!.UID!.Value);
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
        return View("Create", m);
    }
    //TODO:b 404.(found) Add POST: Administration/Transaction/Release (and flap your pages)
    [HttpPost]
    public async Task<IActionResult> Release([FromForm] int id, Guid uid)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _transactionService.ReleaseBookAsync(LibraryId, Librarian.Id, uid, id);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ModelState.AddModelError("Loxa", Q.Loxa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ModelState.AddModelError("", Q.Loxa);
            }
        }
        //TODO:b 465. Update Temp data to Model Errors
        TempData["Error"] = GetModelErrors();
        return RedirectToAction(nameof(Index));
    }
    //TODO:b 406. Add GET/POST: Administration/Transaction/Loan
    [HttpGet]
    public async Task<IActionResult> Loan(Guid? uid = null, string? memberId = null)
    {
        var m = new Create
        {
            Member = memberId != null ? await _transactionService.GetMemberAsync<MemberDTO>(LibraryId, memberId) : default!,
            Book = uid.HasValue ? await _transactionService.GetBookAsync<BookDTO>(LibraryId, uid.Value) : default!,
            Type = Data.Organisation.Inventory.Transaction.TransactionType.Loan
        };
        return View("Create", m);
    }
    [HttpPost]
    public async Task<IActionResult> Loan(Create m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _transactionService.LoanBookAsync(LibraryId, Librarian.Id, m.Member!.Id!, m.Book!.UID!.Value);
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
        return View("Create", m);
    }
    //TODO:b 408. Add GET/POST: Administration/Transaction/Return
    [HttpGet]
    public async Task<IActionResult> Return(int id)
      => View(await _transactionService.GetTransactionAsync<TransactionDTO>(LibraryId,Librarian.Id, id,TemporalisLibri.ActiveTransactionsAndRestorations));
    [HttpPost]
    public async Task<IActionResult> Return(TransactionDTO m)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _transactionService.ReturnBookAsync(LibraryId, Librarian.Id, m.MemberId!, m.Book!.UID!.Value, m.Id);
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
        return View((await _transactionService.GetTransactionAsync<TransactionDTO>(LibraryId,Librarian.Id,m.Id,TemporalisLibri.ActiveTransactionsAndRestorations)).Adapt(m));
    }
}