using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tela.Controllers;
namespace Tela.Models;
//TODO:b 320. Add LoxaAttribute to Models
//Typically I'd create a namespace for custom attributes, but for simplicity I'm adding it to the Models namespace
public class LoxaAttribute :  ActionFilterAttribute
{
    //TODO:b 321. Override OnActionExecuted to add Loxa to ModelState if it's not valid
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if(!context.ModelState.IsValid)
        {
            context.ModelState.AddModelError("Loxa",Q.Loxa);
        }   
        //TODO:b 449. Extract bibliotheca claim to ViewData (get library/tenant id)
        if(context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            (context.Controller as Controller)!.ViewData["bibliotheca"] = context.HttpContext.User.FindFirst("bibliotheca")!.Value; 
        }
        base.OnActionExecuted(context);
    }
}