using System.Text;

namespace Employee.Mgmt.Services;

public class CreateIssuer
{
    private readonly ILogger<CreateIssuer> _logger;
    private readonly string? _swiyuIssuerMgmtUrl;
    private readonly HttpClient _httpClient;

    public CreateIssuer(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _swiyuIssuerMgmtUrl = configuration["SwiyuIssuerMgmtUrl"];
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<CreateIssuer>();
    }

    public async Task<string> IssuerCredentialAsync()
    {
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
        // 
        _logger.LogInformation("Creating issuer");

        var statusRegistryUrl = "https://status-reg.trust-infra.swiyu-int.admin.ch/api/v1/statuslist/8cddcd3c-d0c3-49db-a62f-83a5299214d4.jwt";
        var vcType = "damienbod-vc";

        var json = GetBody(statusRegistryUrl,  vcType);

    }

    /// <summary>
    /// TODO: Requires the accepted issuer
    /// </summary>
    private static string GetBody(string statusRegistryUrl, string vcType)
    {
        var json = $$"""
             {
               "metadata_credential_supported_id": [
                 "{{vcType}}"
               ],
               "credential_subject_data": {
                 "firstName": "Test FirstName",
                 "lastName": "Test LastName",
                 "birthDate": "01.01.2025"
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

    public async Task<string> GetIssuanceStatus(string id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(
            $"{_swiyuIssuerMgmtUrl}/api/v1/credentials{id}/status");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Could not create verification presentation {vp}", error);

        throw new Exception(error);
    }
}
