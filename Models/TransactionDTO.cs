//TODO:b 171. Add Required using statements to TransactionDTO
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Tela.Models;
//TODO:b 170. Add TransactionDTO Class
public class TransactionDTO 
{
    //TODO:b 172. Add Required Attribute to TransactionDTO Id
    [Required(ErrorMessage = "Transaction Required: Absentia Transactionis Vetita")]
    public int Id { get; set; }
    public string? MemberId { get; set; }
    public MemberDTO? Member { get; set; }
    public string? LibrarianId { get; set; }
    public LibrarianDTO? Librarian { get; set; }
    public Guid UID { get; set; }
    public BookDTO? Book { get; set; }
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    //TODO:b 173. Add Json Converter for TransactionType Enum to ensure correct serialization
    [JsonConverter(typeof(JsonStringEnumConverter<Data.Organisation.Inventory.Transaction.TransactionType>))]
    public Data.Organisation.Inventory.Transaction.TransactionType Type { get; set; }
}