using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Event;

public class NetworkEvent : IEvent
{
    public string Nick { get; set; } = null!;
    public long Uin { get; set; }
    public string Content { get; set; } = null!;
    public string RouteValue => "$event:network";
}
