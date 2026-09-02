using System.ComponentModel.DataAnnotations;

namespace SportSys.Contract.Models.inventory;

public class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(200, ErrorMessage = "Název nesmí přesáhnout 200 znaků.")]
    [Display(Name = "Název")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "Popis nesmí přesáhnout 500 znaků.")]
    [Display(Name = "Popis")]
    public string? Description { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; } = true;
}

public class LocationListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
}

public class LocationSelectItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
