using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models.inventory;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.Inventory.Pages.Loans;

public class IndexModel : PageModel
{
    private readonly LoanService _service;

    public IndexModel(LoanService service)
    {
        _service = service;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public LoanFilter Filter { get; set; } = new();

    public List<LoanListItem> Loans { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Loans = await _service.GetLoansAsync(Filter, ct);
    }
}
