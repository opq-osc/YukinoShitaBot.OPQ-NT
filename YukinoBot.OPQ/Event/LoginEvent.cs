using System.Text.Json;
using System.Text.Json.Serialization;
using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Event;

public class LoginEvent : IEvent
{
    public string Nick { get; set; } = null!;
    public long Uin { get; set; }

    [JsonIgnore]
    public DateTime Time { get; set; }

    [JsonIgnore]
    public string Route => "$event:login";

    [JsonIgnore]
    public string EventId => "$event:login";

    [JsonIgnore]
    public string? EventParameter => JsonSerializer.Serialize(this);
}

