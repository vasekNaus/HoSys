using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportSys.Contract.Models;
using SportSys.Contract.Services;

namespace SportSys.Razor.Areas.sport.Pages.IceRink;

public class EditModel : PageModel
{
    private readonly IceRinkService _service;

    public EditModel(IceRinkService service)
    {
        _service = service;
    }

    [BindProperty]
    public IceRinkDto Input { get; set; } = new();

    public bool IsNew => Input.Id == 0;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
            return Page();

        var dto = await _service.GetByIdAsync(id.Value);
        if (dto is null)
            return NotFound();

        Input = dto;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (Input.Id == 0)
            await _service.CreateAsync(Input);
        else
            await _service.UpdateAsync(Input);

        return RedirectToPage("Index");
    }
}
