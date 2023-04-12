using System.Text.Json.Serialization;
using YukinoBot.OPQ.Data;

namespace YukinoBot.OPQ.Event;

public class MessageEvent : Entity.Message
{
    [JsonPropertyName("MsgHead")]
    public MessageHead MessageHead { get; set; } = null!;

    [JsonPropertyName("MsgBody")]
    public MessageBody? MessageBody { get; set; }
}

