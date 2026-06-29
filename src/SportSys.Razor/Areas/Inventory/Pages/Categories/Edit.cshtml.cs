using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Categories;

public class EditModel : PageModel
{
    private readonly CategoryService _categoryService;

    public EditModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [BindProperty]
    public CategoryModel Input { get; set; } = new();

    public List<CategorySelectItem> ParentCategories { get; set; } = [];

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, int? parentCategoryId, CancellationToken ct)
    {
        ParentCategories = await _categoryService.GetSelectListAsync(excludeId: id, ct);

        if (id is null)
        {
            // Nová kategorie – předvyplnit nadřazenou pokud přišla z tlačítka "+" ve stromě
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
            ParentCategories = await _categoryService.GetSelectListAsync(
                excludeId: Input.Id == 0 ? null : Input.Id, ct);
            return Page();
        }

        if (Input.Id == 0)
            await _categoryService.CreateAsync(Input, ct);
        else
            await _categoryService.UpdateAsync(Input, ct);

        return RedirectToPage("Index");
    }
}
