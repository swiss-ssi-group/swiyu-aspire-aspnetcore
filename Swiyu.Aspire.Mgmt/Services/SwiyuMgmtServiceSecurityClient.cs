using Duende.IdentityModel.Client;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Swiyu.Aspire.Mgmt.Services;

public class SwiyuMgmtServiceSecurityClient
{
    /// <summary>
    /// MSAL
    /// </summary>
    public static async Task<string> RequestTokenAsync(
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var clientId = configuration["SwiyuManagementClientId"];
        var clientSecret = configuration["SwiyuManagementClientSecretEntra"];
        var authority = configuration["SwiyuManagementAuthority"];
        var scope = configuration["SwiyuManagementScope"];

        var app = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(authority)
            .Build();

        try
        {
            var authResult = await app
                .AcquireTokenForClient([scope])
                .ExecuteAsync(cancellationToken);

            logger.LogInformation("Token acquired. ExpiresOn={ExpiresOn}", authResult.ExpiresOn);
            return authResult.AccessToken;
        }
        catch (MsalServiceException ex)
        {
            logger.LogError(ex,
                "MSAL service error while requesting token. CorrelationId={CorrelationId}, ErrorCode={ErrorCode}, StatusCode={StatusCode}, Authority={Authority}, Scope={Scope}",
                ex.CorrelationId, ex.ErrorCode, ex.StatusCode, authority, scope);
            throw;
        }
        catch (MsalClientException ex)
        {
            logger.LogError(ex,
                "MSAL client error while requesting token. CorrelationId={CorrelationId}, ErrorCode={ErrorCode}, Authority={Authority}, Scope={Scope}",
                ex.CorrelationId, ex.ErrorCode, authority, scope);
            throw;
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(ex,
                "Token request timed out. Authority={Authority}, Scope={Scope}",
                authority, scope);
            throw;
        }
    }

    /// <summary>
    /// OAuth
    /// </summary>
    public static async Task<TokenResponse> RequestTokenOAuthAsync(IConfiguration configuration)
    {
        var client = new HttpClient();

        // TODO use address
        var disco = await client.GetDiscoveryDocumentAsync(configuration["OAuthIssuerUrl"]);

        if (disco.IsError) throw new Exception(disco.Error);

        var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "swiyu-client",
            // Client assertions are better
            ClientSecret = "--from secrets vault--",
            Scope = "swiyu",
        });

        if (response.IsError) throw new Exception(response.Error);

        return response;
    }
}
