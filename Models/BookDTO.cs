//TODO:b 151. Add Required using statements to BookDTO
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tela.Data.Organisation.Inventory;
namespace Tela.Models;
//TODO:b 150. Add BookDTO Class 
public class BookDTO
{
    //TODO:b 152. Add Required Attribute to BootDTO UID
    [Required(ErrorMessage = "Book Required: Absentia Libri Vetita")]
    public Guid? UID { get; set; } = default!;
    //TODO:b 153. Add Json Converter for State Enum to ensure correct serialization
    [JsonConverter(typeof(JsonStringEnumConverter<State.Types>))]
    public State.Types? State { get; set; }
    public int ReferenceId { get; set; }
    public string? Title { get; set; }
    public string? ISBN { get; set; }
    public int TransactionCount { get; set; }
    public int LoanCount { get; set; }
    public int HoldCount { get; set; }
    public int RestorationCount { get; set; }
    public bool IsOnLoan { get; set; }
    public bool IsOnHold { get; set; }
    public bool IsBeingRestored { get; set; }
    public bool IsAvailable { get; set; }
}