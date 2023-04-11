using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Event;

public class LoginEvent : IEvent
{
    public string Nick { get; set; } = null!;
    public long Uin { get; set; }

    public string RouteValue => "$event:login";
}

