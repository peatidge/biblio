using Tela.Data.Core;
namespace Tela.Data.Organisation.Inventory;
//TODO:a 37. Add Reference Entity
public class Reference
{
    private Reference() { }
    public Reference(Library library, string isbn, string title,string author, int books = 0)
    {
        Title = title;
        ISBN = isbn;
        Author = author;
        Library = library;
        //TODO:a 42. Synthesize N Books based on provided parameter argument
        for (int i = 0; i < books; i++)
        {
            Books.Add(new Book(this));
        }
    }
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int LibraryId{ get; private set; }
    public Library Library { get; private set; } = default!;
    //TODO:a 41. Add Books Collection Navigation Property
    public List<Book> Books { get; private set; } = new();
    //TODO:b 103. Add Indexer to Reference to retrieve Book by UID
    public Book this[Guid uid] => Books.FirstOrDefault(b => b.UID == uid) ?? throw new DomainRuleException("Hey Book, where are you? (knowhere videtur)");
    //TODO:b 104. Add Filtered Books collection for only available books
    public List<Book> Available => Books.Where(b => b.IsAvailable).ToList();
    //TODO:b 105. Add Count of available books
    public int AvailableCount => Available.Count;
    //TODO:b 106. Add IsAvailable derived property that checks available count
    public bool IsAvailable => AvailableCount > 0;
    //TODO:b 107. Add Filtered Books collection for only unavailable books
    public List<Book> Unavailable => Books.Where(b => !b.IsAvailable).ToList();
    //TODO:b 108. Add Count of unavailable books
    public int UnavailableCount => Unavailable.Count(b => b.State == State.Types.Unavailable);
    //TODO:b 109. Add Filtered Books collection for only on loan books
    public List<Book> OnLoan => Books.Where(b => b.IsOnLoan).ToList();
    //TODO:b 110. Add Count of on loan books
    public int OnLoanCount => Books.Count(b => b.IsOnLoan);
    //TODO:b 111. Add Filtered Books collection for only on hold books
    public List<Book> OnHold => Books.Where(b => b.IsOnHold).ToList();
    //TODO:b 112. Add Count of on hold books
    public int OnHoldCount => Books.Count(b => b.IsOnHold);
    //TODO:b 113. Add Filtered Books collection for only being restored books
    public List<Book> BeingRestored => Books.Where(b => b.IsBeingRestored).ToList();
    //TODO:b 114. Add Count of being restored books
    public int BeingRestoredCount => Books.Count(b => b.IsBeingRestored);
    //TODO:b 374. Add RegisterBook method to Reference
    public Book RegisterBook()
    {
        var book = new Book(this);
        Books.Add(book);
        return book;
    }
}