using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee.Mgmt.Pages;

public class CreateCredentialVerifierModel : PageModel
{
    private readonly CreateVerificationPresentation _createVerificationPresentation;

    public CreateCredentialVerifierModel(CreateVerificationPresentation createVerificationPresentation)
    {
        _createVerificationPresentation = createVerificationPresentation;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        var presentation = _createVerificationPresentation.CreateVerificationCredentialAsync();

        // TODO
    }
}
