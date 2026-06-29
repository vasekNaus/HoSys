using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Locations;

public class IndexModel : PageModel
{
    private readonly LocationService _service;

    public IndexModel(LocationService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? NameFilter { get; set; }

    public List<LocationListItem> Locations { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Locations = await _service.GetAllAsync(NameFilter, ct);
    }
}
