using System.Text.Json.Serialization;

namespace Swiyu.Aspire.Mgmt.Services;

public class CredentialIssuerModel
{
    [JsonPropertyName("management_id")]
    public string management_id { get; set; } = null!;
    [JsonPropertyName("offer_deeplink")]
    public string offer_deeplink { get; set; } = null!;
}