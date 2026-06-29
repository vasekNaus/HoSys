using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Loans;

public class EditModel : PageModel
{
    private readonly LoanService _service;

    public EditModel(LoanService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    public LoanDetail? Loan { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
    {
        Loan = await _service.GetLoanDetailAsync(id, ct);
        if (Loan is null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostReturnItemAsync(int id, int loanId, CancellationToken ct)
    {
        await _service.ReturnItemAsync(loanId, ct);
        StatusMessage = "Položka byla vrácena.";
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostReturnAllAsync(int id, CancellationToken ct)
    {
        await _service.ReturnAllAsync(id, ct);
        StatusMessage = "Všechny položky byly vráceny.";
        return RedirectToPage(new { id });
    }
}
