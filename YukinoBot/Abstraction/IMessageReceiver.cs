namespace YukinoBot.Abstraction;

public interface IMessageReceiver<TMessage> : IMessageReceiver where TMessage : IMessage
{
    event EventHandler<TMessage>? OnMessageGeneric;
}

public interface IMessageReceiver
{
    event EventHandler<IMessage>? OnMessage;
    event EventHandler<IEvent>? OnEvent;

    Task Start();
    void Stop();
}

