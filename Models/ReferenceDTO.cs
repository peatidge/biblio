namespace Tela.Models;
//TODO:b 166. Add Required using statements to ReferenceDTO
public class ReferenceDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int LibraryId { get; set; }
    public int BookCount { get; set; }
    public bool IsAvailable { get; set; }
    public int AvailableCount { get; set; }
    public int UnavailableCount { get; set; }
    public int OnLoanCount { get; set; }
    public int OnHoldCount { get; set; }
    public int BeingRestoredCount { get; set; }

}