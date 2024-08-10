using System.ComponentModel.DataAnnotations;
namespace Tela.Areas.Administration.Models.Member;
//TODO:b 294. Add Administraion Member Create Model
public class Create
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name Required")]
    public string? FirstName { get; set; } = default!;
    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last Name Required")]
    public string? LastName { get; set; } = default!;
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email Required")]
    public string? Email { get; set; } = default!;
}