namespace YukinoBot.Abstraction;

public interface IMessageBuilder<TMessage> : IMessageBuilder where TMessage : IMessage
{
    new TMessage Build();
}

public interface IMessageBuilder
{
    IMessage Build();

    IMessageBuilder WithContent(string content);
    IMessageBuilder AddMedia(IMedia media);
    IMessageBuilder AtUser(IUser user);
    IMessageBuilder SendTo(IUser user);
}
