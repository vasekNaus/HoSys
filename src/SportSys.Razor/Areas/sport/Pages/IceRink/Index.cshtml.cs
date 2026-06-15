using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.IceRink;

public class IndexModel : PageModel
{
    private readonly IceRinkService _service;

    public IndexModel(IceRinkService service)
    {
        _service = service;
    }

    public List<IceRinkDto> IceRinks { get; set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        IceRinks = await _service.GetAllAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteAsync(id);
        StatusMessage = "Zimní stadion byl odstraněn.";
        return RedirectToPage();
    }
}
