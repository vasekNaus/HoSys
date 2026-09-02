using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.Season;

public class EditModel : PageModel
{
    private readonly SeasonService _service;

    public EditModel(SeasonService service)
    {
        _service = service;
    }

    [BindProperty]
    public SeasonEditDto Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id is null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var startYear = today.Month >= 7 ? today.Year : today.Year - 1;
            Input.From = new DateOnly(startYear, 7, 1);
            Input.To = new DateOnly(startYear + 1, 6, 30);
            return Page();
        }

        var dto = await _service.GetByIdAsync(id.Value, ct);
        if (dto is null)
            return NotFound();

        Input = dto;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        if (Input.Id == 0)
        {
            await _service.CreateAsync(Input, ct);
            StatusMessage = "Sezóna byla vytvořena.";
        }
        else
        {
            await _service.UpdateAsync(Input, ct);
            StatusMessage = "Sezóna byla uložena.";
        }

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostSetActiveAsync(bool isActive, CancellationToken ct)
    {
        await _service.SetActiveAsync(Input.Id, isActive, ct);
        StatusMessage = isActive ? "Sezóna byla aktivována." : "Sezóna byla zneaktivněna.";
        return RedirectToPage("Index");
    }
}
