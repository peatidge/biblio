using System.ComponentModel.DataAnnotations;
namespace Tela.Areas.Administration.Models.Reference;
//TODO:b 359. Add Reference Edit model to Administration area
public class Edit : Create
{
    [Required]
    public int Id { get; set; }
}