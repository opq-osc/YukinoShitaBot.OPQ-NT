namespace YukinoBot.OPQ.Event.Base;

public class SocketEvent
{
    public EventRaw CurrentPacket { get; set; } = null!;
    public long CurrentQQ { get; set; }
}
