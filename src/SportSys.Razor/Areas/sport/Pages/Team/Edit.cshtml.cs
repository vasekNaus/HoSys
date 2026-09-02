using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.Team;

public class EditModel : PageModel
{
    private readonly TeamService _service;
    private readonly IceRinkService _iceRinkService;

    public EditModel(TeamService service, IceRinkService iceRinkService)
    {
        _service = service;
        _iceRinkService = iceRinkService;
    }

    [BindProperty]
    public TeamDto Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public List<SelectListItem> HomeIceRinkIdItems { get; private set; } = [];

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id is not null)
        {
            var dto = await _service.GetByIdAsync(id.Value, ct);
            if (dto is null)
                return NotFound();

            Input = dto;
        }

        await LoadIceRinksAsync(ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadIceRinksAsync(ct);
            return Page();
        }

        if (Input.Id == 0)
        {
            await _service.CreateAsync(Input, ct);
            StatusMessage = "Tým byl vytvořen.";
        }
        else
        {
            await _service.UpdateAsync(Input, ct);
            StatusMessage = "Tým byl uložen.";
        }

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostSetActiveAsync(bool isActive, CancellationToken ct)
    {
        await _service.SetActiveAsync(Input.Id, isActive, ct);
        StatusMessage = isActive ? "Tým byl aktivován." : "Tým byl zneaktivněn.";
        return RedirectToPage("Index");
    }

    private async Task LoadIceRinksAsync(CancellationToken ct)
    {
        var iceRinks = await _iceRinkService.GetSelectListAsync(Input.HomeIceRinkId, ct);
        HomeIceRinkIdItems =
        [
            new SelectListItem("— nevybráno —", string.Empty),
            .. iceRinks.Select(r => new SelectListItem(r.Name, r.Id.ToString())),
        ];
    }
}
