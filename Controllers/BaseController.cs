//TODO:b 219. Add using statements to BaseController
using Microsoft.AspNetCore.Mvc;
using Tela.Data.Core;
using Tela.Models;
namespace Tela.Controllers;
//TODO:b 218. Add BaseController
[Loxa] //TODO:b 322. (323 in site.scss) Decorate BaseController with LoxaAttribute
public abstract class BaseController : Controller
{
    protected readonly ApplicationDbContext _context;
    protected readonly ILogger<BaseController> _logger;
    public BaseController(ApplicationDbContext context, ILogger<BaseController> logger)
    {
        _context = context;
        _logger = logger;
    }
    //TODO:b 464. Add GetModelErrors method to BaseController
    public string GetModelErrors() => string.Join("<hr/>",ModelState.SelectMany(m => m.Value!.Errors).Select(e => e.ErrorMessage).ToArray()); 
}
