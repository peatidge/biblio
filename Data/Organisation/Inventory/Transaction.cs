namespace Tela.Data.Organisation.Inventory;
//TODO:a 43. Add Transaction Entity
public abstract class Transaction
{
    //TODO:a 44. Add TransactionType Enumeration (Hold, Loan)
    public enum TransactionType { Hold, Loan }
    protected Transaction() { }
    public Transaction(Librarian librarian, Member member, Book book, DateTime start, DateTime end)
    {
        Book = book;
        BookUID = book.UID;
        Librarian = librarian; 
        Member = member;
        Start = start;
        End = end;
        Version = Guid.NewGuid();
    }
    public int Id { get; protected set; }
    public string MemberId { get; protected set; } = default!;
    public Member Member { get; protected set; } = default!;
    public string LibrarianId { get; protected set; } = default!;
    public Librarian Librarian { get; protected set; } = default!;
    public DateTime Start { get; set; } = default!;
    public DateTime End { get; protected set; } = default!;
    public Guid Version { get; set; }
    public bool IsDeleted { get; private set; }
    public Guid BookUID { get; set; }
    public Book Book { get; set; } = default!;
    public abstract TransactionType Type { get; }
    //TODO:a 95. Add Finalise method to set End and Version properties of transaction
    public void Finalise(Book book)
    {
        End = DateTime.Now;
        Version = Guid.NewGuid();
    }
}