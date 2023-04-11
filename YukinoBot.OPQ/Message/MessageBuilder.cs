using YukinoBot.Abstraction;
using YukinoBot.OPQ.Data;
using YukinoBot.OPQ.Net;

namespace YukinoBot.OPQ.Message;

public class MessageBuilder : IMessageBuilder
{
    internal string Content { get; set; } = string.Empty;
    internal IList<MultiMedia>? Images { get; set; }
    internal MultiMedia? Voice { get; set; }
    internal IList<Uin>? AtList { get; set; }

    internal long Uid { get; set; }
    internal int ToType { get; set; }
    internal long? GroupCode { get; set; }

    public IMessageBuilder At(long uid)
    {
        AtList ??= new List<Uin>();
        AtList.Add(new Uin { UserId = uid });
        return this;
    }

    public IMessageBuilder AddImage(string key)
    {
        Images ??= new List<MultiMedia>();
        return this;
    }

    public IMessageBuilder WithContent(string content)
    {
        this.Content = content;
        return this;
    }

    public IMessageBuilder WithVoice(string key)
    {
        this.Voice = new MultiMedia();
        return this;
    }

    public IOutMessage Build()
    {
        return new SendMessageRequest
        {
            CgiRequest = new OutMessage()
            {
                Images = Images,
                Content = Content,
                ToUin = Uid,
                AtUinLists = AtList,
                Voice = Voice,
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
