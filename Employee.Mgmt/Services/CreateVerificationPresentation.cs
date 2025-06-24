using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Employee.Mgmt.Services;

public class CreateVerificationPresentation
{
    private readonly ILogger<CreateVerificationPresentation> _logger;
    private readonly string? _swiyuVerifierMgmtUrl;
    private readonly HttpClient _httpClient;

    public CreateVerificationPresentation(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuVerifierMgmtUrl = configuration["SwiyuVerifierMgmtUrl"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<CreateVerificationPresentation>();
    }

    public async Task<string> CreateVerificationCredentialAsync()
    {
        _logger.LogInformation("Creating verification presentation");

        //var acceptedIssuerDid = "";
        var inputDescriptorsId = Guid.NewGuid().ToString();
        var presentationDefinitionId = Guid.NewGuid().ToString();

        var json = GetBody(inputDescriptorsId, presentationDefinitionId);

        //curl - X POST http://localhost:8082/api/v1/verifications \
        //       -H "accept: application/json" \
        //       -H "Content-Type: application/json" \
        //       -d '

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _httpClient.PostAsync(
            $"{_swiyuVerifierMgmtUrl}/api/v1/verifications", jsonContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        return jsonResponse;
    }

    /// <summary>
    /// TODO: Requires the accepted issuer
    /// </summary>
    private static string GetBody(string inputDescriptorsId, string presentationDefinitionId)
    {
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
                                             "const": "my-test-vc"
                                         }
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
}
