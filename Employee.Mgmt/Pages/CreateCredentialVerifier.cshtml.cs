using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee.Mgmt.Pages;

public class CreateCredentialVerifierModel : PageModel
{
    private readonly CreateVerificationPresentation _createVerificationPresentation;
    private readonly string? _swiyuOid4vpUrl;

    public string? QrCodeUrl { get; set; } = "https://damienbod.com";

    public CreateCredentialVerifierModel(CreateVerificationPresentation createVerificationPresentation,
        IConfiguration configuration)
    {
        _createVerificationPresentation = createVerificationPresentation;
        _swiyuOid4vpUrl = configuration["SwiyuOid4vpUrl"];
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        var presentation = _createVerificationPresentation.CreateVerificationCredentialAsync();

        // verification_url
        // TODO
    }
}
