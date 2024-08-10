//TODO:b 256. Add Member Area to Tela project (only Controllers and Views folders required)
//TODO:b 258. Add using statements to MemberController
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tela.Controllers;
using Tela.Data.Core;
namespace Tela.Areas.Member.Controllers;
//TODO:b 257. Add MemberController with Area and Authorize attributes
[Area("Member"),Authorize(Roles ="MEMBER")]
public abstract class MemberController : BaseController
{
    public MemberController(ApplicationDbContext context, ILogger<BaseController> logger) : base(context, logger)
    {
    } 
}