namespace Tela.Data.Records;
//TODO:b 426. Add LibraryBookStatus to Records
public class LibraryBookStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Count { get; set; }
    public int OnLoan { get; set; }
    public int OnHold { get; set; }
    public int BeingRestored { get; set; }
    public int Available { get; set; }
}
