using System.Text.Json.Serialization;

namespace Swiyu.Aspire.Mgmt.Services;

public class StatusModel
{
    [JsonPropertyName("id")]
    public string id { get; set; }
    [JsonPropertyName("status")]
    public string status { get; set; }
}