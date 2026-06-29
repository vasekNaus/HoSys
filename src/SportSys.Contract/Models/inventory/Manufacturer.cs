using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models.inventory;

public class Manufacturer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(200, ErrorMessage = "Název nesmí přesáhnout 200 znaků.")]
    [Display(Name = "Název")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "URL nesmí přesáhnout 500 znaků.")]
    [Url(ErrorMessage = "Zadejte platnou URL adresu.")]
    [Display(Name = "Web")]
    public string? Website { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; } = true;
}
