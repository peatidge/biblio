using Microsoft.AspNetCore.Mvc;
using Tela.Data.Core;
namespace Tela.Controllers;
//TODO:a 33. Add LudusController to orchestrate the creation and persistence of Library, Roles, Librarian and Member
public class LudusController : Controller //OOPS:Typo,updated Ludas to Ludus (Views folder name also updated) 
{
    //TODO:a 34. Add Ludus DI
    private readonly Ludus _ludas; 
    //TODO:a 35. Add Constructor with Ludus DI
    public LudusController(Ludus ludas)
    {
        _ludas = ludas;
    }
    //TODO:a 36. Add Evolvere Action & associated View to intitial and display result of orchestration 
    public async Task<IActionResult> Evolvere()
    {
        try
        {
            return View(await _ludas.EvolvereAsync());
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", $"{ex.Message}, Oopsy, no worries let's jog on🏃");
            return View();
        }    
    }
    [HttpGet]
    public async Task<IActionResult> MutareDominium() => View(await _ludas.MutareDominium());
    [HttpGet]
    //TODO:b 179. Add MapToAdapt Action & View
    public async Task<IActionResult> MapToAdapt() => View(await _ludas.MapToAdapt());  
}