namespace Tela.Data.Organisation.Inventory;
//TODO:a 48. Add Restoration Entity
public class Restoration
{
    private Restoration() { }
    public Restoration(Librarian librarian, Book book, DateTime start, DateTime end)
    {
        Librarian = librarian;
        LibrarianId = librarian.Id;
        Book = book;
        Start = start;
        End = end;
    }
    public int Id { get; private set; }
    public string LibrarianId { get; private set; } = default!;
    public Librarian Librarian { get; private set; } = default!;
    public Guid BookUID { get; private set; }
    public Book Book { get; private set; } = default!;
    public DateTime Start { get; set; } = default!;
    public DateTime End { get; set; } = default!;
}