namespace YukinoBot.Abstraction;

public interface IMessageReceiver
{
    event EventHandler<IInMessage>? OnMessage;
    event EventHandler<IEvent>? OnEvent;

    Task Start();
    void Stop();
}

