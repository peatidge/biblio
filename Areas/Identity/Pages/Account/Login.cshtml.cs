//TODO:b 276. Update using statements in Login.cshtml.cs 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tela.Data.Organisation;
namespace Tela.Areas.Identity.Pages.Account;
//TODO:b 275. Add LoginModel and Login.cshtml to Tela project 
//See: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-8.0&tabs=visual-studio
//NOTE: This page has been heavily modified to simplify the example project, with features such as external login, return url removed. If time permits, advanced features can be added back in later
public class LoginModel : PageModel
{
    private readonly SignInManager<LibraryUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    public LoginModel(SignInManager<LibraryUser> signInManager, ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }
    //TODO:b 278. Add Input property to LoginModel as bound data model for login form
    [BindProperty]
    public InputModel Input { get; set; } = default!;
    //TODO:b 279. Add ErrorMessage property to LoginModel for displaying login errors
    [TempData]
    public string ErrorMessage { get; set; } = default!;
    //TODO:b 277. Add InputModel nested class to LoginModel
    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    //TODO:b 280. Add OnGet method to LoginModel for handling login form http get requests
    public void OnGet()
    {
        //TODO:b 286. Implement OnGet method to check error message and set default value for RememberMe (true)
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }
        //use null coelescing operator instantiate Input if null and set RememberMe to true
        Input = Input ?? new InputModel { RememberMe = true }; 
    }

    //TODO:b 289. Add OnPostAsync method to LoginModel for handling login form http post requests
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email) ?? throw new InvalidOperationException();
                //redirect the user to the relevant area based on their role
                if (_signInManager.UserManager.IsInRoleAsync(user, Roles.LIBRARIAN.ToString()).Result)
                {
                    return LocalRedirect("/Administration");
                }
                else if (_signInManager.UserManager.IsInRoleAsync(user, Roles.MEMBER.ToString()).Result)
                {
                    return LocalRedirect("/Member");
                }
            }
            ModelState.AddModelError(string.Empty,"Invalid login attempt.");
        }
        return Page();
    }
}