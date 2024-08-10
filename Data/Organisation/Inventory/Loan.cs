namespace Tela.Data.Organisation.Inventory;
//TODO:a 46. Add Loan Entity
public class Loan : Transaction
{
    protected Loan() : base() { }
    public Loan(Librarian librarian, Member member, Book book) : base(librarian, member,book, DateTime.Now, DateTime.MaxValue) { }
    public override TransactionType Type => TransactionType.Loan;

}