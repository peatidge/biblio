using Microsoft.AspNetCore.Mvc;
using Tela.Controllers;
using Tela.Data.Core;
namespace Tela.Areas.Member.Controllers;
//TODO:b 259. Add HomeController, using statements and Index action to Member Area
//*Note: extends MemberController
public class HomeController : MemberController
{
    public HomeController(ApplicationDbContext context, ILogger<BaseController> logger) : base(context, logger)
    {
    }
    //TODO:b 439. Add lemur.js to project
    //TODO:b 440. Add lemur folder + infinite possibilities to images folder
    //TODO:b 441. Add lemur styles to style.scss
    public IActionResult Index() => View();  
}