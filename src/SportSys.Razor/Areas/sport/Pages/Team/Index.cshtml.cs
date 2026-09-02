using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.Team;

public class IndexModel : PageModel
{
    private readonly TeamService _service;

    public IndexModel(TeamService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? IsActive { get; set; } = true;

    public List<TeamListItem> Teams { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Teams = await _service.GetAllAsync(Search, IsActive, ct);
    }

    public async Task<IActionResult> OnPostSetActiveAsync(
        int id,
        bool isActive,
        CancellationToken ct)
    {
        await _service.SetActiveAsync(id, isActive, ct);
        StatusMessage = isActive ? "Tým byl aktivován." : "Tým byl zneaktivněn.";
        return RedirectToPage(new { Search, IsActive });
    }
}
