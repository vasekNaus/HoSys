using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Manufacturers;

public class IndexModel : PageModel
{
    private readonly ManufacturerService _service;

    public IndexModel(ManufacturerService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? NameFilter { get; set; }

    public List<Manufacturer> Manufacturers { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Manufacturers = await _service.GetAllAsync(NameFilter, ct);
    }
}
