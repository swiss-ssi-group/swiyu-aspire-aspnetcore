using Swiyu.Aspire.Mgmt.Services;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.Codecrete.QrCodeGenerator;
using System.Text.Json;

namespace Swiyu.Aspire.Mgmt.Pages;

public class CreateStatusListModel : PageModel
{
    private readonly IssuerService _issuerService;

    [BindProperty]
    public string? StatusList { get; set; } = null;

    public CreateStatusListModel(IssuerService issuerService)
    {
        _issuerService = issuerService;
    }

    public void OnGet()
    {
        // default HTTP GET is required
    }


    public async Task OnPostAsync()
    {
        StatusList = await _issuerService.CreateStatusList();
        return;
    }
}
