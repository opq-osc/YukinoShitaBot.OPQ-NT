namespace YukinoBot.Abstraction;

public interface IMessageBuilder
{
    IMessageBuilder CreateFriend(long sendToUid);
    IMessageBuilder CreateSession(long sendToUid, long groupId);
    IMessageBuilder CreateGroup(long groupId);
    IMessageBuilder WithContent(string content);
    IMessageBuilder WithVoice(string key);
    IMessageBuilder AddImage(string key);
    IMessageBuilder At(long uid);

    IOutMessage Build();
}
