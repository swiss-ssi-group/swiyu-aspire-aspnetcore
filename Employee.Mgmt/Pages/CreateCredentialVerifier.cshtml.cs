using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee.Mgmt.Pages;

public class CreateCredentialVerifierModel : PageModel
{
    private readonly CreateVerificationPresentation _createVerificationPresentation;
    private readonly string? _swiyuOid4vpUrl;

    [BindProperty]
    public string? QrCodeUrl { get; set; } = "{OID4VP_URL}/api/v1/request-object/c35b9d28-1155-4d63-8433-abf741293f2a";

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

    public void OnPost()
    {
        var presentation = _createVerificationPresentation.CreateVerificationCredentialAsync();

        // verification_url
        // TODO

    }

    private void result ()
    {
        //{
        //  "id": "c35b9d28-1155-4d63-8433-abf741293f2a",
        //	"request_nonce": "zTFz/9xUiMNnXkwS+BdxY0SFzQ9W2CAR",
        //	"state": "PENDING",
        //	"presentation_definition": {
        //                "id": "d7f849b8-0318-4f06-bf3b-40c08ff53f0b",
        //		"name": "Test Verification",
        //		"purpose": "We want to test a new Verifier",
        //		"format": { },
        //		"input_descriptors": [

        //            {
        //                    "id": "1bdc9432-75c9-4217-8e44-47158eebe53b",
        //				"format": {
        //                        "vc+sd-jwt": {
        //                            "sd-jwt_alg_values": [
        //                                "ES256"
        //                            ],
        //						"kb-jwt_alg_values": [
        //                            "ES256"
        //                        ]
    
        //                    }
        //                    },
        //				"constraints": {
        //                        "format": { },
        //					"fields": [

        //                        {
        //                            "path": [
        //                                "$.vct"
        //                            ],
        //							"filter": {
        //                                "type": "string",
        //								"const": "my-test-vc"

        //                            }
        //                        },
        //						{
        //                            "path": [
        //                                "$.lastName"
        //                            ]

        //                        },
        //						{
        //                            "path": [
        //                                "$.birthDate"
        //                            ]

        //                        }
        //					]
        //				}
        //                }
        //		]
        //	},
        //	"verification_url": "${OID4VP_URL}/api/v1/request-object/c35b9d28-1155-4d63-8433-abf741293f2a"
        //}
    }
}
