using System.Text.Json.Nodes;

namespace YukinoBot.OPQ.Event.Base;

public class EventRaw
{
    public string EventName { get; set; } = null!;
    public JsonObject EventData { get; set; } = null!;
}
