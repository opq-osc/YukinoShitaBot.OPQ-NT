namespace YukinoBot.Abstraction;

public interface IMessageBuilderFactory
{
    IMessageBuilder Create();
    IMessageBuilder CreateReply(IMessage message);
}
