using System.Text.Json;
using System.Text.Json.Serialization;
using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Event;

public class NetworkEvent : IEvent
{
    public string Nick { get; set; } = null!;
    public long Uin { get; set; }
    public string Content { get; set; } = null!;

    [JsonIgnore]
    public DateTime Time { get; set; }

    [JsonIgnore]
    public string Route => "$event:network";

    [JsonIgnore]
    public string EventId => "$event:network";

    [JsonIgnore]
    public string? EventParameter => JsonSerializer.Serialize(this);
}
