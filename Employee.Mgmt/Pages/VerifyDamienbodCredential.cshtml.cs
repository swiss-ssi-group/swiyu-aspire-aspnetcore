using Employee.Mgmt.Services;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.Codecrete.QrCodeGenerator;
using System.Text.Json;

namespace Employee.Mgmt.Pages;

public class VerifyDamienbodCredentialModel : PageModel
{
    private readonly VerificationService _verificationService;
    private readonly string? _swiyuOid4vpUrl;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = "";

    [BindProperty]
    public byte[] QrCodePng { get; set; } = [];

    public VerifyDamienbodCredentialModel(VerificationService verificationService,
        IConfiguration configuration)
    {
        _verificationService = verificationService;
        _swiyuOid4vpUrl = configuration["SwiyuOid4vpUrl"];
        QrCodeUrl = QrCodeUrl.Replace("{OID4VP_URL}", _swiyuOid4vpUrl);
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        var presentation = await _verificationService
            .CreateDamienbodVerificationPresentationAsync();

        var data = JsonSerializer.Deserialize<CreateVerificationPresentationModel>(presentation);
        // verification_url
        QrCodeUrl = data!.verification_url;

        var qrCode = QrCode.EncodeText(data!.verification_url, QrCode.Ecc.Quartile);
        QrCodePng = qrCode.ToPng(20, 4, MagickColors.Black, MagickColors.White);
    }
}
