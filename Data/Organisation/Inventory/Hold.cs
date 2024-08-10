namespace Tela.Data.Organisation.Inventory;
//TODO:a 47. Add Hold Entity
public class Hold : Transaction
{
    protected Hold() : base() { }
    public Hold(Librarian librarian, Member member,Book book) : base(librarian, member, book, DateTime.Now, DateTime.MaxValue) { }
    public override TransactionType Type => TransactionType.Hold;
}