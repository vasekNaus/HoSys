using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models;

public class SeasonEditDto : IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(50, ErrorMessage = "Název nesmí přesáhnout 50 znaků.")]
    [Display(Name = "Název")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Datum začátku je povinné.")]
    [DataType(DataType.Date)]
    [Display(Name = "Od")]
    public DateOnly From { get; set; }

    [Required(ErrorMessage = "Datum konce je povinné.")]
    [DataType(DataType.Date)]
    [Display(Name = "Do")]
    public DateOnly To { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From > To)
            yield return new ValidationResult(
                "Datum začátku sezóny nesmí být pozdější než datum konce.",
                [nameof(From), nameof(To)]);
    }
}

public class SeasonListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public bool IsActive { get; set; }
}
