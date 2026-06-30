using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Categories;

public class EditModel : PageModel
{
    private readonly CategoryService _categoryService;
    private readonly ItemKindService _itemKindService;

    public EditModel(CategoryService categoryService, ItemKindService itemKindService)
    {
        _categoryService = categoryService;
        _itemKindService = itemKindService;
    }

    [BindProperty]
    public CategoryModel Input { get; set; } = new();

    public List<CategorySelectItem> ParentCategories { get; set; } = [];
    public List<ItemKindListItem> AvailableKinds { get; set; } = [];

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, int? parentCategoryId, CancellationToken ct)
    {
        await LoadSelectListsAsync(excludeId: id, ct);

        if (id is null)
        {
            if (parentCategoryId.HasValue)
                Input.ParentCategoryId = parentCategoryId.Value;
            return Page();
        }

        var model = await _categoryService.GetByIdAsync(id.Value, ct);
        if (model is null) return NotFound();

        Input = model;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync(excludeId: Input.Id == 0 ? null : Input.Id, ct);
            return Page();
        }

        // Odebrat prázdné řádky (uživatel kliknul Přidat, ale nevybral druh)
        Input.CategoryKindInputs.RemoveAll(k => k.SizesText == null && k.ItemKind == 0);

        if (Input.Id == 0)
            await _categoryService.CreateAsync(Input, ct);
        else
            await _categoryService.UpdateAsync(Input, ct);

        return RedirectToPage("Index");
    }

    private async Task LoadSelectListsAsync(int? excludeId, CancellationToken ct)
    {
        ParentCategories = await _categoryService.GetSelectListAsync(excludeId: excludeId, ct: ct);
        AvailableKinds = await _itemKindService.GetAllAsync(ct);
    }
}
