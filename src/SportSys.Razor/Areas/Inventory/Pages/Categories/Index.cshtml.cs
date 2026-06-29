using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly CategoryService _service;

    public IndexModel(CategoryService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    // Flat seznam (zakomentovaný pohled – filtrovatelný)
    [BindProperty(SupportsGet = true)]
    public string? NameFilter { get; set; }

    public List<CategoryListItem> Categories { get; set; } = [];

    // Strom (výchozí pohled)
    public List<CategoryTreeNode> Tree { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        // Flat seznam (pro zakomentovaný pohled s filtrem)
        Categories = await _service.GetAllAsync(NameFilter, ct);

        // Strom (vždy plný – filtr se neaplikuje)
        Tree = await _service.GetTreeAsync(ct);
    }
}
