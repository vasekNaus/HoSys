using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;
using SportSys.Database.Enums;
using SportSys.Database.Models.inventory;

namespace SportSys.Razor.Areas.Inventory.Pages.Items;

public class EditModel : PageModel
{
    private readonly InventoryItemService _service;
    private readonly CategoryService _categoryService;
    private readonly ManufacturerService _manufacturerService;
    private readonly LocationService _locationService;
    private readonly ItemKindService _itemKindService;

    public EditModel(
        InventoryItemService service,
        CategoryService categoryService,
        ManufacturerService manufacturerService,
        LocationService locationService,
        ItemKindService itemKindService)
    {
        _service = service;
        _categoryService = categoryService;
        _manufacturerService = manufacturerService;
        _locationService = locationService;
        _itemKindService = itemKindService;
    }

    [BindProperty]
    public InventoryItemForm Input { get; set; } = new();

    public bool IsNew => Input.Id == 0;

    public List<SelectListItem> CategorySelectList { get; set; } = [];
    public List<SelectListItem> ManufacturerSelectList { get; set; } = [];
    public List<SelectListItem> LocationSelectList { get; set; } = [];
    public List<SelectListItem> StatusSelectList { get; set; } = [];
    public List<SelectListItem> ItemKindSelectList { get; set; } = [];
    /// <summary>JSON mapa categoryId → CategoryKind[] pro dynamické dropdowny v JS.</summary>
    public string CategoryKindsJson { get; set; } = "{}";

    public async Task<IActionResult> OnGetAsync(int? id, string? itemType, CancellationToken ct)
    {
        if (id is not null)
        {
            var form = await _service.GetByIdAsync(id.Value, ct);
            if (form is null) return NotFound();
            Input = form;
        }
        else
        {
            Input.ItemType = itemType is "Asset" or "Equipment" ? itemType : "Equipment";
        }

        await LoadSelectListsAsync(ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync(ct);
            return Page();
        }

        if (Input.Id == 0)
            await _service.CreateAsync(Input, ct);
        else
            await _service.UpdateAsync(Input, ct);

        TempData["StatusMessage"] = $"Položka \"{Input.Name}\" byla úspěšně uložena.";
        return RedirectToPage("Index");
    }

    private async Task LoadSelectListsAsync(CancellationToken ct)
    {
        var categories = await _categoryService.GetSelectListAsync(ct: ct);
        CategorySelectList = [new("— vyberte —", ""), .. categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()))];

        // Mapa categoryId → [{id, itemKind, sizes}] pro dynamické dropdowny v JS
        var kindsMap = categories
            .Where(c => c.CategoryKinds is { Length: > 0 })
            .ToDictionary(c => c.Id, c => c.CategoryKinds!.Select(k => new
            {
                id = (int)k.ItemKind,
                itemKind = k.ItemKind.ToString(),
                sizes = k.Sizes
            }).ToArray());
        CategoryKindsJson = JsonSerializer.Serialize(kindsMap);

        var manufacturers = await _manufacturerService.GetAllAsync(ct: ct);
        ManufacturerSelectList = [new("— nevybráno —", ""), .. manufacturers.Where(m => m.IsActive).Select(m => new SelectListItem(m.Name, m.Id.ToString()))];

        var locations = await _locationService.GetSelectListAsync(ct: ct);
        LocationSelectList = [new("— nevybráno —", ""), .. locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()))];

        StatusSelectList = [.. Enum.GetValues<EItemStatus>().Select(s => new SelectListItem(s.GetDisplayName(), ((int)s).ToString()))];

        var kinds = await _itemKindService.GetAllAsync(ct);
        ItemKindSelectList = [new("— nevybráno —", ""), .. kinds.Select(k => new SelectListItem(k.Name, k.Id.ToString()))];
    }
}
