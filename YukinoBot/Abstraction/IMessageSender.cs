namespace YukinoBot.Abstraction;

public interface IMessageSender
{
    void Send(IOutMessage message);
}

