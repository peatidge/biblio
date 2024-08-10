//TODO:b 221. Add using statements to AdministrationController
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tela.Controllers;
using Tela.Data.Core;
using Tela.Data.Organisation;
namespace Tela.Areas.Administration.Controllers;
[Area("Administration"),Authorize(Roles ="LIBRARIAN")]
//TODO:b 220. Add AdministrationController with Area and Authorize attributes
public abstract class AdministrationController : BaseController
{
    public AdministrationController(ApplicationDbContext context, ILogger<AdministrationController> logger) : base(context,logger){}
    //TODO:b 222. Add Librarian and LibraryId properties to AdministrationController for reuse in extended controllers
    protected Librarian Librarian => _context.Users.OfType<Librarian>().First(l => l.UserName == User.Identity!.Name);
    protected int LibraryId => Librarian.LibraryId;
}