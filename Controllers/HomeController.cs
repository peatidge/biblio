using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tela.Data.Core;
using Tela.Models;
namespace Tela.Controllers;
//TODO:b 452. Update home controller to inherit from BaseController (inherits LoxaAttribute)
public class HomeController : BaseController
{
    public HomeController(ApplicationDbContext context, ILogger<BaseController> logger) : base(context, logger)
    {
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();
    }
    public  ActionResult Index() => View();
    [HttpGet]
    public IActionResult Privacy() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}