namespace Tela.Data.Records;
//TODO:b 472. Add TranceActionsStat class to Tela.Data.Records namespace
public class TranceActionsStat
{
    public string ISBN { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Books { get; set; }
    public int Holds { get; set; }
    public decimal HoldPercentage { get; set; }
    public int Loans{ get; set; }
    public decimal LoanPercentage { get; set; }
    public int Restorations { get; set; }
    public decimal RestorationPercentage { get; set; }
}