namespace YukinoBot.Abstraction;

public interface IMessageReceiver
{
    void OnMessage<T>(T message) where T : IInMessage;
    void OnEvent<T>(T message) where T : IEvent;

    Task Start();
}

