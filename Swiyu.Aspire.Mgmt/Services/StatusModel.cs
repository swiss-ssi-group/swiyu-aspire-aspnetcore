using System.Text.Json.Serialization;

namespace Employee.Mgmt.Services;

public class StatusModel
{
    [JsonPropertyName("id")]
    public string id { get; set; }
    [JsonPropertyName("status")]
    public string status { get; set; }
}