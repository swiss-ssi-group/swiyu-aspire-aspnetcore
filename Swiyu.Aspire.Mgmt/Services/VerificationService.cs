using Duende.IdentityModel.Client;
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
    private readonly IConfiguration _configuration;

    public VerificationService(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuVerifierMgmtUrl = configuration["SwiyuVerifierMgmtUrl"];
        _issuerId = configuration["ISSUER_ID"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<VerificationService>();
        _configuration = configuration;
    }

    /// <summary>
    /// curl - X POST http://localhost:8082/management/api/verifications \
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

        var json = GetBetaIdVerificationPresentationBodyV4(inputDescriptorsId,
            presentationDefinitionId, acceptedIssuerDid, "betaid-sdjwt");

        return await SendCreateVerificationPostRequest(json);
    }

    /// <summary>
    /// curl - X POST http://localhost:8082/management/api/verifications \
    ///       -H "accept: application/json" \
    ///       -H "Content-Type: application/json" \
    ///       -d '
    /// </summary>
    public async Task<string> CreateDamienbodVerificationPresentationAsync()
    {
        _logger.LogInformation("Creating verification presentation");

        var inputDescriptorsId = Guid.NewGuid().ToString();
        var presentationDefinitionId = "00000000-0000-0000-0000-000000000000"; // Guid.NewGuid().ToString();

        var json = GetDataForLocalCredential(inputDescriptorsId, presentationDefinitionId, _issuerId!, "damienbod-vc");

        return await SendCreateVerificationPostRequest(json);
    }

    public async Task<VerificationManagementModel?> GetVerificationStatus(string verificationId)
    {
        var idEncoded = HttpUtility.UrlEncode(verificationId);
        using HttpResponseMessage response = await _httpClient.GetAsync(
            $"{_swiyuVerifierMgmtUrl}/management/api/verifications/{idEncoded}");

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
        var accessToken = await SwiyuMgmtServiceSecurityClient.RequestTokenAsync(_configuration);

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.SetBearerToken(accessToken);
        var response = await _httpClient.PostAsync($"{_swiyuVerifierMgmtUrl}/management/api/verifications", jsonContent);

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
                 "response_mode": "direct_post.jwt",

                 "verification_purpose": {
                   "scope": "ch.identity",
                   "purpose_name": {
                     "default": "Identity verification"
                   },
                   "purpose_description": {
                     "default": "Used to verify the identity of an individual"
                   }
                 },
                 "dcql_query": {
                   "credentials": [
                     {
                       "id": "{{presentationDefinitionId}}",
                       "format": "dc+sd-jwt",
                       "meta": {
                         "vct_values": ["damienbod-vc"]
                       },
                       "claims": [
             		     { "path": [ "$.family_name" ] },
                         { "path": [ "$.given_name" ] },     
             		     { "path": [ "$.birth_date" ] }
                       ],
                       "require_cryptographic_holder_binding": true
                     }
                   ]
                 }
             }
             """;

        return json;
    }

    private static string GetBetaIdVerificationPresentationBodyV4(string inputDescriptorsId, string presentationDefinitionId, string acceptedIssuerDid, string vcType)
    {
        var json = $$"""
             {
                 "accepted_issuer_dids": [ "{{acceptedIssuerDid}}" ],
                 "jwt_secured_authorization_request": true,
                 "response_mode": "direct_post.jwt",
                 "verification_purpose": {
                   "scope": "ch.identity",
                   "purpose_name": {
                     "default": "Identity verification"
                   },
                   "purpose_description": {
                     "default": "Used to verify the identity of an individual"
                   }
                 },
                 "dcql_query": {
                   "credentials": [
                     {
                       "id": "{{presentationDefinitionId}}",
                       "format": "dc+sd-jwt",
                       "meta": {
                         "vct_values": ["betaid-sdjwt"]
                       },
                       "claims": [
                         { "path": [ "$.birth_date" ] },
             		     { "path": [ "$.given_name" ] },
             		     { "path": [ "$.family_name" ] },
             		     { "path": [ "$.birth_place" ] }
                       ],
                       "require_cryptographic_holder_binding": true
                     }
                   ]
                 }
             }
             """;

        return json;
    }
}
