using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Employee.Mgmt.Pages;

public class CreateCredentialVerifierModel : PageModel
{
    private readonly CreateVerificationPresentation _createVerificationPresentation;
    private readonly string? _swiyuOid4vpUrl;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = "{OID4VP_URL}/api/v1/request-object/${VerificationId}";

    public CreateCredentialVerifierModel(CreateVerificationPresentation createVerificationPresentation,
        IConfiguration configuration)
    {
        _createVerificationPresentation = createVerificationPresentation;
        _swiyuOid4vpUrl = configuration["SwiyuOid4vpUrl"];
        QrCodeUrl = QrCodeUrl.Replace("{OID4VP_URL}", _swiyuOid4vpUrl);
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        var presentation = await _createVerificationPresentation
            .CreateVerificationCredentialAsync();

        var data = JsonSerializer.Deserialize<CreateVerificationPresentationModel>(presentation);
        // verification_url
        QrCodeUrl = data!.verification_url;
    }
}
