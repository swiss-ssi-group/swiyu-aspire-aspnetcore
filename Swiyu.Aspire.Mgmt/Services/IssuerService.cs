using System.Text;
using System.Text.Json;

namespace Swiyu.Aspire.Mgmt.Services;

public class IssuerService
{
    private readonly ILogger<IssuerService> _logger;
    private readonly string? _swiyuIssuerMgmtUrl;
    private readonly HttpClient _httpClient;

    public IssuerService(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuIssuerMgmtUrl = configuration["SwiyuIssuerMgmtUrl"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<IssuerService>();
    }

    public async Task<string> IssuerCredentialAsync(PayloadCredentialData payloadCredentialData)
    {
        _logger.LogInformation("Issuer credential for data");

        var statusRegistryUrl = "https://status-reg.trust-infra.swiyu-int.admin.ch/api/v1/statuslist/8cddcd3c-d0c3-49db-a62f-83a5299214d4.jwt";
        var vcType = "damienbod-vc";

        var json = GetBody(statusRegistryUrl, vcType, payloadCredentialData);

        //  curl - X POST http://localhost:8084/api/v1/credentials \
        // -H "accept: */*" \
        // -H "Content-Type: application/json" \
        // -d '

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _httpClient.PostAsync(
            $"{_swiyuIssuerMgmtUrl}/api/v1/credentials", jsonContent);

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
    private static string GetBody(string statusRegistryUrl, string vcType, PayloadCredentialData payloadCredentialData)
    {
        var json = $$"""
             {
               "metadata_credential_supported_id": [
                 "{{vcType}}"
               ],
               "credential_subject_data": {
                 "firstName": "{{payloadCredentialData.FirstName}}",
                 "lastName": "{{payloadCredentialData.LastName}}",
                 "birthDate": "{{payloadCredentialData.BirthDate}}"
               },
               "offer_validity_seconds": 86400,
               "credential_valid_until": "2030-01-01T19:23:24Z",
               "credential_valid_from": "2025-01-01T18:23:24Z",
               "status_lists": [
                 "{{statusRegistryUrl}}"
               ]
             }
             """;

        return json;
    }

    public async Task<StatusModel?> GetIssuanceStatus(string id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(
            $"{_swiyuIssuerMgmtUrl}/api/v1/credentials/{id}/status");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if(jsonResponse == null)
            {
                _logger.LogError("GetIssuanceStatus no data returned from Swiyu");
                return new StatusModel { id="none", status="ERROR"};
            }

            return JsonSerializer.Deserialize<StatusModel>(jsonResponse);
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Could not create verification presentation {vp}", error);

        throw new Exception(error);
    }
}
