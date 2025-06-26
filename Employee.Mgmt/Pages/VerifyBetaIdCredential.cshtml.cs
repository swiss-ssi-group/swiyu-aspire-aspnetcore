using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.Codecrete.QrCodeGenerator;
using System.Text.Json;

namespace Employee.Mgmt.Pages;

public class VerifyBetaIdCredentialModel : PageModel
{
    private readonly CreateVerificationPresentation _createVerificationPresentation;
    private readonly string? _swiyuOid4vpUrl;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = "";

    [BindProperty]
    public byte[] QrCodePng { get; set; } = [];

    public VerifyBetaIdCredentialModel(CreateVerificationPresentation createVerificationPresentation,
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
            .CreateBetaIdVerificationPresentationAsync();

        var data = JsonSerializer.Deserialize<CreateVerificationPresentationModel>(presentation);
        // verification_url
        QrCodeUrl = data!.verification_url;

        var qrCode = QrCode.EncodeText(data!.verification_url, QrCode.Ecc.Quartile);
        QrCodePng = qrCode.ToPng(20, 4);
    }
}
