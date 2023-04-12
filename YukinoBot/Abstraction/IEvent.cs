namespace YukinoBot.Abstraction;

public interface IEvent
{
    string Route { get; }
    string EventId { get; }
    string? EventParameter { get; }
    DateTime Time { get; }
}

