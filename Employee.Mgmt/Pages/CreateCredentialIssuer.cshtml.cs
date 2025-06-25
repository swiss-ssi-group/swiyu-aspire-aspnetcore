using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Employee.Mgmt.Pages;

public class CreateCredentialIssuerModel : PageModel
{
    private readonly CreateIssuer _createIssuer;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = null;

    public CreateCredentialIssuerModel(CreateIssuer createIssuer)
    {
        _createIssuer = createIssuer;
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        var vci = await _createIssuer.IssuerCredentialAsync();

        var data = JsonSerializer.Deserialize<CredentialIssuerModel>(vci);

        QrCodeUrl = data!.offer_deeplink;
    }
}
