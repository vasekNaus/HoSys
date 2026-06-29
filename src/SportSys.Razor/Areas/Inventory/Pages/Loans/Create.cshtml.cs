using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Loans;

public class CreateModel : PageModel
{
    private readonly LoanService _service;

    public CreateModel(LoanService service)
    {
        _service = service;
    }

    public List<MemberSelectItem> Members { get; set; } = [];

    [BindProperty]
    public CreateLoan Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        Members = await _service.GetActiveMembersAsync(ct);
        return Page();
    }

    /// <summary>GET handler pro vyhledání položky dle inventárního čísla (QR skener / AJAX).</summary>
    public async Task<IActionResult> OnGetLookupAsync(string inventoryNumber, CancellationToken ct)
    {
        var result = await _service.LookupItemAsync(inventoryNumber, ct);
        return new JsonResult(result);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid || Input.InventoryNumbers.Count == 0)
        {
            if (Input.InventoryNumbers.Count == 0)
                ModelState.AddModelError("", "Přidejte alespoň jednu položku.");

            Members = await _service.GetActiveMembersAsync(ct);
            return Page();
        }

        try
        {
            var groupId = await _service.CreateLoanAsync(Input, ct);
            return RedirectToPage("Edit", new { id = groupId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            Members = await _service.GetActiveMembersAsync(ct);
            return Page();
        }
    }
}
