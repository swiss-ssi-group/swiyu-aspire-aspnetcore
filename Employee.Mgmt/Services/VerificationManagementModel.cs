using System.Text.Json.Serialization;

namespace Employee.Mgmt.Services;

public class VerificationManagementModel
{
    [JsonPropertyName("id")]
    public string id { get; set; }

    [JsonPropertyName("request_nonce")]
    public string request_nonce { get; set; }

    /// <summary>
    /// PENDING, SUCCESS, FAILED
    /// </summary>
    [JsonPropertyName("state")]
    public string state { get; set; }

    [JsonPropertyName("presentation_definition")]
    public object? presentation_definition { get; set; }

    [JsonPropertyName("wallet_response")]
    public VerificationWalletResponseModel? wallet_response { get; set; }

    [JsonPropertyName("verification_url")]
    public string verification_url { get; set; }
}
