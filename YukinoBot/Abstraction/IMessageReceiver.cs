namespace YukinoBot.Abstraction;

public interface IMessageReceiver
{
    event EventHandler<IMessage>? OnMessage;
    event EventHandler<IEvent>? OnEvent;

    Task Start();
    void Stop();
}

