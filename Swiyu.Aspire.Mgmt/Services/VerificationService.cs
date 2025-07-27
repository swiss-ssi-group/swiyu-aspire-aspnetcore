using System.Text;
using System.Text.Json;
using System.Web;

namespace Swiyu.Aspire.Mgmt.Services;

public class VerificationService
{
    private readonly ILogger<VerificationService> _logger;
    private readonly string? _swiyuVerifierMgmtUrl;
    private readonly string? _issuerId;
    private readonly HttpClient _httpClient;

    public VerificationService(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuVerifierMgmtUrl = configuration["SwiyuVerifierMgmtUrl"];
        _issuerId = configuration["ISSUER_ID"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<VerificationService>();
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
        var presentationDefinitionId = "00000000-0000-0000-0000-000000000000"; // Guid.NewGuid().ToString();

        var json = GetBetaIdVerificationPresentationBody(inputDescriptorsId,
            presentationDefinitionId, acceptedIssuerDid, "betaid-sdjwt");

        return await SendCreateVerificationPostRequest(json);
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
        var presentationDefinitionId = "00000000-0000-0000-0000-000000000000"; // Guid.NewGuid().ToString();

        var json = GetDataForLocalCredential(inputDescriptorsId,
           presentationDefinitionId, _issuerId!, "damienbod-vc");

        return await SendCreateVerificationPostRequest(json);
    }

    public async Task<VerificationManagementModel?> GetVerificationStatus(string verificationId)
    {
        var idEncoded = HttpUtility.UrlEncode(verificationId);
        using HttpResponseMessage response = await _httpClient.GetAsync(
            $"{_swiyuVerifierMgmtUrl}/api/v1/verifications/{idEncoded}");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (jsonResponse == null)
            {
                _logger.LogError("GetVerificationStatus no data returned from Swiyu");
                return null;
            }

            //  state: PENDING, SUCCESS, FAILED
            return JsonSerializer.Deserialize<VerificationManagementModel>(jsonResponse);
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Could not create verification presentation {vp}", error);

        throw new ArgumentException(error);
    }

    private async Task<string> SendCreateVerificationPostRequest(string json)
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

        throw new ArgumentException(error);
    }

    private static string GetDataForLocalCredential(string inputDescriptorsId, string presentationDefinitionId, string issuer, string vcType)
    {
        // jwt_secured_authorization_request disabled, need docs for this
        var json = $$"""
             {
                 "accepted_issuer_dids": [ "{{issuer}}" ],
                 "jwt_secured_authorization_request": true,
                 "presentation_definition": {
                     "id": "{{presentationDefinitionId}}",
                     "name": "Verification",
                     "purpose": "Verify damienbod VC",
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
             			                "path": [ "$.vct" ],
             			                "filter": {
             				                "type": "string",
             				                "const": "{{vcType}}"
             			                }
             		                },
                                    {
                                        "path": [ "$.firstName" ]
                                    },
                                    {
                                        "path": [ "$.lastName" ]
                                    },
             		                {
             			                "path": [ "$.birthDate" ]
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

    private static string GetBetaIdVerificationPresentationBody(string inputDescriptorsId, string presentationDefinitionId, string acceptedIssuerDid, string vcType)
    {
        var json = $$"""
             {
                 "accepted_issuer_dids": [ "{{acceptedIssuerDid}}" ],
                 "jwt_secured_authorization_request": true,
                 "presentation_definition": {
                     "id": "{{presentationDefinitionId}}",
                     "name": "Verification",
                     "purpose": "Verify using Beta ID",
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
