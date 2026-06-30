using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;
using SportSys.Database.Enums;
using SportSys.Database.Models.inventory;

namespace SportSys.Razor.Areas.Inventory.Pages.Items;

public class AssetsModel : PageModel
{
    private readonly InventoryItemService _service;
    private readonly CategoryService _categoryService;

    public AssetsModel(InventoryItemService service, CategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public InventoryTypeFilter Filter { get; set; } = new();

    public List<AssetListItem> Items { get; set; } = [];
    public List<SelectListItem> CategorySelectList { get; set; } = [];
    public List<SelectListItem> StatusSelectList { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Items = await _service.GetAssetListAsync(Filter, ct);

        var categories = await _categoryService.GetSelectListAsync(ct: ct);
        CategorySelectList = [new("— vše —", ""), .. categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()))];

        StatusSelectList = [new("— vše —", ""),
            .. Enum.GetValues<EItemStatus>().Select(s => new SelectListItem(s.GetDisplayName(), ((int)s).ToString()))];
    }
}
