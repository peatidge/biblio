using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tela.Models;
namespace Tela.Areas.Administration.Models.Restoration;
//TODO:b 419. Add Schedule to Administration Restoration Models
public class Schedule : IValidatableObject
{
    public BookDTO Book { get;  set; } = default!;
    [DataType(DataType.Date)]
    [Remote(action: "DateIsInFuture", controller: "Restoration",AdditionalFields="start")]
    public DateTime? Start { get; set; } = default!;
    [DataType(DataType.Date)]
    [Remote(action: "DateIsInFuture", controller: "Restoration", AdditionalFields = "end")]
    public DateTime? End { get; set; } = default!;
    public bool HasBook => Book != null && Book?.UID != null;
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Start > End)
        {
            yield return new ValidationResult("Start must precede End (temperal nuance detected)",[nameof(Start)]);
            yield return new ValidationResult("End must succeed Start (detected nuance temperal)", [nameof(Start)]);
        }
    }
}