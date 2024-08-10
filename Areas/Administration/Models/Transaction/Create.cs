using Tela.Models;
namespace Tela.Areas.Administration.Models.Transaction;
//TODO:b 390. Add Create Model to Administration Area Transaction
public class Create
{
    public bool HasBook => Book?.UID != null && Book.UID != Guid.Empty;
    public bool HasMember => Guid.TryParse(Member?.Id,out _);
    public BookDTO? Book { get; set; }
    public MemberDTO? Member { get; set; }
    public Data.Organisation.Inventory.Transaction.TransactionType Type { get; set; } 
}