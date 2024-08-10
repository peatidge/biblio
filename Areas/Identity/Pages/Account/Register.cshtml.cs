using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Services;
namespace Tela.Areas.Identity.Pages.Account;
//TODO:b 453. Add (scaffold & customize) RegisterModel & Register.cshtml.cs 
public class RegisterModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly LibraryService _libraryService;
    private readonly SignInManager<LibraryUser> _signInManager;
    public RegisterModel(ApplicationDbContext context, LibraryService libraryService,SignInManager<LibraryUser> signInManager)
    {
        _context = context;
        _libraryService = libraryService;
        _signInManager = signInManager;
    }
    [BindProperty]
    public InputModel Input { get; set; } = default!;
    public class InputModel
    {
        [Required,Display(Name ="Library")]
        public int LibraryId { get; set; }     
        public SelectList? Libraries { get; set; }
        [Required,Display(Name = "First Name")]
        public string FirstName { get; set; } = default!;
        [Required,Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;
        [Required,EmailAddress,Display(Name = "Email")]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password),Required,StringLength(100, ErrorMessage = "Minimum 1 character"), Display(Name = "Password")]
        public string Password { get; set; } = default!;
        [DataType(DataType.Password),Display(Name = "Confirm"),Compare("Password", ErrorMessage = "Password and confirmation differ")]
        public string ConfirmPassword { get; set; } = default!;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        Input = new InputModel
        {
            Libraries = new SelectList(await _context.Libraries.ToArrayAsync(),nameof(Library.Id),nameof(Library.Name))
        };
        return Page(); 
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            try
            {
                var m = await _libraryService.RegisterMemberAsync(Input.LibraryId, Input.Email, Input.FirstName, Input.LastName, Input.Password);
                await _signInManager.SignInAsync(await _context.Users.FirstAsync(u => u.Id == m.Id),false);
                return LocalRedirect("/Member");
            }
            catch(InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            Input.Libraries = new SelectList(await _context.Libraries.ToArrayAsync(), nameof(Library.Id), nameof(Library.Name));
        }
        return Page();
    }
}