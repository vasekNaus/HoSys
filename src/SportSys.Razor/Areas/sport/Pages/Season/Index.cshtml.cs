using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.Season;

public class IndexModel : PageModel
{
    private readonly SeasonService _service;

    public IndexModel(SeasonService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? IsActive { get; set; } = true;

    public List<SeasonListItem> Seasons { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Seasons = await _service.GetAllAsync(Search, IsActive, ct);
    }

    public async Task<IActionResult> OnPostSetActiveAsync(
        int id,
        bool isActive,
        CancellationToken ct)
    {
        await _service.SetActiveAsync(id, isActive, ct);
        StatusMessage = isActive ? "Sezóna byla aktivována." : "Sezóna byla zneaktivněna.";
        return RedirectToPage(new { Search, IsActive });
    }
}
