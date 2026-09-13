using Swiyu.Aspire.Mgmt.Services;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.Codecrete.QrCodeGenerator;
using System.Text.Json;

namespace Swiyu.Aspire.Mgmt.Pages;

public class CreateCredentialIssuerModel : PageModel
{
    private readonly IssuerService _issuerService;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = null;

    [BindProperty]
    public byte[] QrCodePng { get; set; } = [];

    [BindProperty]
    public string? DeepLink { get; set; } = string.Empty;

    [BindProperty]
    public string? ManagementId { get; set; } = null;

    public CreateCredentialIssuerModel(IssuerService issuerService)
    {
        _issuerService = issuerService;
    }

    public void OnGet()
    {
        // default HTTP GET is required
    }

    /// <summary>
    /// QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High
    /// </summary>
    /// <returns></returns>
    public async Task OnPostAsync()
    {
        var vci = await _issuerService.IssuerCredentialAsync(
            new PayloadCredentialData
            {
                FamilyName = "McGarry",
                GivenName = "Eoin",
                BirthDate = DateTime.UtcNow.ToShortDateString()
            });

        var issueCredential = JsonSerializer.Deserialize<CredentialIssuerModel>(vci);

        var qrCode = QrCode.EncodeText(issueCredential!.offer_deeplink, QrCode.Ecc.Quartile);
        QrCodePng = qrCode.ToPng(20, 4, MagickColors.Black, MagickColors.White);

        QrCodeUrl = issueCredential!.offer_deeplink;
        DeepLink = issueCredential.offer_deeplink;
        ManagementId = issueCredential!.management_id;
    }
}
