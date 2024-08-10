//TODO:b 224. Add using statements to Administration HomeController
using Microsoft.AspNetCore.Mvc;
using Tela.Controllers;
using Tela.Data.Core;
namespace Tela.Areas.Administration.Controllers;
//TODO:b 223. Add HomeController to Administration Area
public class HomeController : AdministrationController
{
    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger) : base(context, logger) {}
    //TODO:b 225. Add Administration Home Index Action and associated view 
    [HttpGet]
    public IActionResult Index()  => View();
    //TODO:b 228 Check access to {host}/Administration after authenticating as Librarian "Lib.Rarian@dobbs"
    //TODO:b 229-245 is Grunt files (Grunfile.js, Grunt\Sass\site.scss, Grunt\Scripts\site.js)
}