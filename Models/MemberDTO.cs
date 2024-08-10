//TODO:b 162. Add Required using statements to MemberDTO
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tela.Data.Organisation;
namespace Tela.Models;
//TODO:b 161. Add MemberDTO Class 
public class MemberDTO
{
    //TODO:b 163. Add Required Attribute to Id to MemberDTO Id
    [Required(ErrorMessage = "Member Required: Absentia Sodalis Bibliothecae Vetita")]
    public string? Id { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? UserName { get; set; } = default!;
    public int? LibraryId { get; set; }
    //TODO:b 164. Add Json Converter for Role Enum to ensure correct serialization
    [JsonConverter(typeof(JsonStringEnumConverter<Roles>))]
    public Roles Role => Roles.MEMBER;
    public int TransactionCount { get; set; }
    public int LoanCount { get; set; }
    public int HoldCount { get; set; }
}