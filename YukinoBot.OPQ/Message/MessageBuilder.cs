using YukinoBot.Abstraction;
using YukinoBot.OPQ.Data;
using YukinoBot.OPQ.Net;

namespace YukinoBot.OPQ.Message;

public class MessageBuilder : IMessageBuilder
{
    private string content = string.Empty;
    private IList<MultiMedia>? images;
    private MultiMedia? voice;
    private IList<Uin>? at;

    internal long Uid { get; set; }
    internal int ToType { get; set; }
    internal long? GroupCode { get; set; }

    public IMessageBuilder At(long uid)
    {
        at ??= new List<Uin>();
        at.Add(new Uin { UserId = uid });
        return this;
    }

    public IMessageBuilder AddImage(string key)
    {
        images ??= new List<MultiMedia>();
        return this;
    }

    public IMessageBuilder WithContent(string content)
    {
        this.content = content;
        return this;
    }

    public IMessageBuilder WithVoice(string key)
    {
        this.voice = new MultiMedia();
        return this;
    }

    public IOutMessage Build()
    {
        return new SendMessageRequest
        {
            CgiRequest = new OutMessage()
            {
                Images = images,
                Content = content,
                ToUin = Uid,
                AtUinLists = at,
                Voice = voice,
                GroupCode = GroupCode,
                ToType= ToType,
            }
        };
    }

    public IMessageBuilder CreateFriend(long sendToUid)
    {
        return new MessageBuilder { ToType = 1, Uid = sendToUid };
    }

    public IMessageBuilder CreateSession(long sendToUid, long groupId)
    {
        return new MessageBuilder { ToType = 3, Uid = sendToUid, GroupCode = groupId };
    }

    public IMessageBuilder CreateGroup(long groupId)
    {
        return new MessageBuilder() { ToType = 2, Uid = groupId };
    }
}
