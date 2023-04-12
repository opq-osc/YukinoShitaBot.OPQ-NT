namespace YukinoBot.Abstraction;

public interface IMessageBuilderFactory
{
    IMessageBuilder Create();
    IMessageBuilder<T> Create<T>() where T : IMessage;
    IMessageBuilder<T> CreateReply<T>(T msg) where T : IMessage;
}
