using System.Text.Json.Serialization;

namespace YukinoBot.OPQ.Data;

public class Uin
{
    [JsonPropertyName("Uin")]
    public long UserId { get; set; }

    public string? Nick { get; set; }
}

