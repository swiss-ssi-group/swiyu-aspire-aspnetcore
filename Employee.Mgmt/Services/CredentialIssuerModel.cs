using System.Text.Json.Serialization;

namespace Employee.Mgmt.Services;

public class CredentialIssuerModel
{
    [JsonPropertyName("management_id")]
    public string management_id { get; set; }
    [JsonPropertyName("offer_deeplink")]
    public string offer_deeplink { get; set; }
}