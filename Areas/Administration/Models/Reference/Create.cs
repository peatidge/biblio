using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Tela.Areas.Administration.Models.Reference;
//TODO:b 348. Add Reference Create model to Administration area
public class Create
{
    [Required]
    public string Title { get; set; } = default!;
    //NOTE: Rember to enable client side validation in Program.cs for remote validation to work
    [RegularExpression(@"^\d{3}-\d{10}$"),Remote("IsUniqueISBN","Reference","Administration")]
    public string ISBN { get; set; } = default!;
    [Required]
    public string Author { get; set; } = default!;
    [Range(0,10)]
    public int Books { get; set; }
}