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

    public async Task<string> CreateVerificationCredentialAsync()
    {
        _logger.LogInformation("Creating verification presentation");

        // from "betaid-sdjwt"
        var acceptedIssuerDid = "did:tdw:QmPEZPhDFR4nEYSFK5bMnvECqdpf1tPTPJuWs9QrMjCumw:identifier-reg.trust-infra.swiyu-int.admin.ch:api:v1:did:9a5559f0-b81c-4368-a170-e7b4ae424527";

        var inputDescriptorsId = Guid.NewGuid().ToString();
        var presentationDefinitionId = Guid.NewGuid().ToString();
        var vcType = "damienbod-vc"; //"betaid-sdjwt"; // "damienbod-vc"

        var json = GetBody(inputDescriptorsId, presentationDefinitionId, acceptedIssuerDid, vcType);

        //curl - X POST http://localhost:8082/api/v1/verifications \
        //       -H "accept: application/json" \
        //       -H "Content-Type: application/json" \
        //       -d '

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _httpClient.PostAsync(
            $"{_swiyuVerifierMgmtUrl}/api/v1/verifications", jsonContent);

        if(response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Could not create verification presentation {vp}", error);

        throw new Exception(error);
    }

    /// <summary>
    /// TODO: Requires the accepted issuer
    /// </summary>
    private static string GetBody(string inputDescriptorsId, string presentationDefinitionId, string acceptedIssuerDid, string vcType)
    {
        // not using {{acceptedIssuerDid}} for now, TODO add
        // _issuerId
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
}
