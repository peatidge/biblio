//TODO:b 157. Add Required using statements to LibrarianDTO
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tela.Data.Organisation;
namespace Tela.Models;
//TODO:b 156. Add LibrarianDTO Class 
public class LibrarianDTO
{
    //TODO:b 158. Add Required Attribute to Id to LibrarianDTO Id
    [Required(ErrorMessage = "Librarian Required: Absentia bibliothecarii vetita")]
    public string? Id { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? UserName { get; set; } = default!;
    public int? LibraryId { get; set; }
    //TODO:b 159. Add Json Converter for Role Enum to ensure correct serialization
    [JsonConverter(typeof(JsonStringEnumConverter<Roles>))]
    public Roles Role => Roles.LIBRARIAN;
    public int TransactionCount { get; set; }
    public int LoanCount { get; set; }
    public int HoldCount { get; set; }
    public int RestorationCount { get; set; }
}