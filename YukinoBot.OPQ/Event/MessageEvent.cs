using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Data;
using YukinoBot.OPQ.Message;

namespace YukinoBot.OPQ.Event;

public class MessageEvent : IInMessage
{
    [JsonPropertyName("MsgHead")]
    public MessageHead MessageHead { get; set; } = null!;

    [JsonPropertyName("MsgBody")]
    public MessageBody MessageBody { get; set; } = null!;

    // TODO 多模态Content支持
    public string Content => MessageBody.Content;  

    private HashSet<long>? atList;
    public HashSet<long> AtList => atList ??= new HashSet<long>(MessageBody.AtUinLists?.Select(x => x.UserId) 
                                              ?? Enumerable.Empty<long>());

    public IMessageBuilder CreateReply()
    {
        return new MessageBuilder
        {
            ToType = MessageHead.FromType,
            Uid = MessageHead.FromUin,
            GroupCode = MessageHead.GroupInfo?.GroupCode
        };
    }

    public bool TryGetImages([NotNullWhen(true)] out IList<string>? images)
    {
        images = MessageBody.Images?
                            .Where(x => !string.IsNullOrEmpty(x.Url))
                            .Select(x => x.Url ?? string.Empty).ToList();
        return images is not null;
    }

    public bool TryGetVideo([NotNullWhen(true)] out string video)
    {
        throw new NotImplementedException();
    }

    public bool TryGetVoice([NotNullWhen(true)] out string voice)
    {
        throw new NotImplementedException();
    }
}

