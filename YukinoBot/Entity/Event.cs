using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class Event : IEvent
{
    public string Route { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;

    public string? EventParameter { get; set; }

    public DateTime Time { get; set; }
}
