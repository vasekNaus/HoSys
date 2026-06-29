using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Manufacturers;

public class EditModel : PageModel
{
    private readonly ManufacturerService _service;

    public EditModel(ManufacturerService service)
    {
        _service = service;
    }

    [BindProperty]
    public Manufacturer Input { get; set; } = new();

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id is null) return Page();

        var model = await _service.GetByIdAsync(id.Value, ct);
        if (model is null) return NotFound();

        Input = model;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        if (Input.Id == 0)
            await _service.CreateAsync(Input, ct);
        else
            await _service.UpdateAsync(Input, ct);

        return RedirectToPage("Index");
    }
}
