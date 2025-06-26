using System.Text;

namespace Employee.Mgmt.Services;

public class CreateVerificationPresentation
{
    private readonly ILogger<CreateVerificationPresentation> _logger;
    private readonly string? _swiyuVerifierMgmtUrl;
    private readonly string? _issuerId;
    private readonly HttpClient _httpClient;

    public CreateVerificationPresentation(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuVerifierMgmtUrl = configuration["SwiyuVerifierMgmtUrl"];
        _issuerId = configuration["ISSUER_ID"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<CreateVerificationPresentation>();
    }

    /// <summary>
    /// curl - X POST http://localhost:8082/api/v1/verifications \
    ///       -H "accept: application/json" \
    ///       -H "Content-Type: application/json" \
    ///       -d '
    /// </summary>
    public async Task<string> CreateBetaIdVerificationPresentationAsync()
    {
        _logger.LogInformation("Creating verification presentation");

        // from "betaid-sdjwt"
        var acceptedIssuerDid = "did:tdw:QmPEZPhDFR4nEYSFK5bMnvECqdpf1tPTPJuWs9QrMjCumw:identifier-reg.trust-infra.swiyu-int.admin.ch:api:v1:did:9a5559f0-b81c-4368-a170-e7b4ae424527";

        var inputDescriptorsId = Guid.NewGuid().ToString();
        var presentationDefinitionId = Guid.NewGuid().ToString();

        var json = GetBetaIdVerificationPresentationBody(inputDescriptorsId,
            presentationDefinitionId, acceptedIssuerDid, "betaid-sdjwt");

        return await SendPostRequest(json);
    }

    /// <summary>
    /// curl - X POST http://localhost:8082/api/v1/verifications \
    ///       -H "accept: application/json" \
    ///       -H "Content-Type: application/json" \
    ///       -d '
    /// </summary>
    public async Task<string> CreateDamienbodVerificationPresentationAsync()
    {
        _logger.LogInformation("Creating verification presentation");

        var inputDescriptorsId = Guid.NewGuid().ToString();
        var presentationDefinitionId = Guid.NewGuid().ToString();

        var json = GetDataForLocalCredential(inputDescriptorsId,
           presentationDefinitionId, _issuerId!, "damienbod-vc");

        return await SendPostRequest(json);
    }

    private async Task<string> SendPostRequest(string json)
    {
        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(
                    $"{_swiyuVerifierMgmtUrl}/api/v1/verifications", jsonContent);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Could not create verification presentation {vp}", error);

        throw new Exception(error);
    }

    private string GetDataForLocalCredential(string inputDescriptorsId, string presentationDefinitionId, string issuer, string vcType)
    {
        var json = $$"""
             {
                 "accepted_issuer_dids": [ "{{issuer}}" ],
                 "jwt_secured_authorization_request": true,
                 "presentation_definition": {
                     "id": "{{presentationDefinitionId}}",
                     "name": "Test Verification",
                     "purpose": "We want to test a new Verifier",
                     "input_descriptors": [
                         {
                             "id": "{{inputDescriptorsId}}",
                             "format": {
                                 "vc+sd-jwt": {
                                     "sd-jwt_alg_values": [
                                         "ES256"
                                     ],
                                     "kb-jwt_alg_values": [
                                         "ES256"
                                     ]
                                 }
                             },
                             "constraints": {
             	                "fields": [
             		                {
             			                "path": [
             				                "$.vct"
             			                ],
             			                "filter": {
             				                "type": "string",
             				                "const": "{{vcType}}"
             			                }
             		                },
                                    {
                                        "path": [
                                            "$.firstName"
                                        ]
                                    },
                                    {
                                        "path": [
                                            "$.lastName"
                                        ]
                                    },
             		                {
             			                "path": [
             				                "$.birthDate"
             			                ]
             		                }
             	                ]
                             }
                         }
                     ]
                 }
             }
             """;

        return json;
    }

    private string GetBetaIdVerificationPresentationBody(string inputDescriptorsId, string presentationDefinitionId, string acceptedIssuerDid, string vcType)
    {
        // TODO, not working {{acceptedIssuerDid}}
        var json = $$"""
             {
                 "accepted_issuer_dids": [],
                 "jwt_secured_authorization_request": true,
                 "presentation_definition": {
                     "id": "{{presentationDefinitionId}}",
                     "name": "Test Verification",
                     "purpose": "We want to test a new Verifier",
                     "input_descriptors": [
                         {
                             "id": "{{inputDescriptorsId}}",
                             "format": {
                                 "vc+sd-jwt": {
                                     "sd-jwt_alg_values": [
                                         "ES256"
                                     ],
                                     "kb-jwt_alg_values": [
                                         "ES256"
                                     ]
                                 }
                             },
                             "constraints": {
             	                "fields": [
             		                {
             			                "path": [
             				                "$.vct"
             			                ],
             			                "filter": {
             				                "type": "string",
             				                "const": "{{vcType}}"
             			                }
             		                },
             		                {
             			                "path": [
             				                "$.birth_date"
             			                ]
             		                }
             	                ]
                             }
                         }
                     ]
                 }
             }
             """;

        return json;
    }
}
