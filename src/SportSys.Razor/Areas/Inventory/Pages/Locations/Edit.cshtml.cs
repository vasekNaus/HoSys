using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Locations;

public class EditModel : PageModel
{
    private readonly LocationService _service;

    public EditModel(LocationService service)
    {
        _service = service;
    }

    [BindProperty]
    public Location Input { get; set; } = new();

    public List<LocationSelectItem> ParentLocations { get; set; } = [];

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        ParentLocations = await _service.GetSelectListAsync(excludeId: id, ct);

        if (id is null) return Page();

        var model = await _service.GetByIdAsync(id.Value, ct);
        if (model is null) return NotFound();

        Input = model;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ParentLocations = await _service.GetSelectListAsync(
                excludeId: Input.Id == 0 ? null : Input.Id, ct);
            return Page();
        }

        if (Input.Id == 0)
            await _service.CreateAsync(Input, ct);
        else
            await _service.UpdateAsync(Input, ct);

        return RedirectToPage("Index");
    }
}
