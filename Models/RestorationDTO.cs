namespace Tela.Models;
//TODO:b 168. Add RestorationDTO Class
public class RestorationDTO
{
    public BookDTO Book { get; set; } = default!;
    public DateTime Start { get; set; } = default!;
    public DateTime End { get; set; } = default!;
}
