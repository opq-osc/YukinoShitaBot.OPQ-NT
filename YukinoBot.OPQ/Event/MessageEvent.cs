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
    public MessageBody? MessageBody { get; set; }

    [JsonIgnore]
    public string Content { get; internal set; } = string.Empty;

    private HashSet<long>? atList;

    [JsonIgnore]
    public HashSet<long> AtList => atList ??= new HashSet<long>(MessageBody?.AtUinLists?.Select(x => x.UserId) 
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

    public IMessageBuilder CreateRepeat()
    {
        return new MessageBuilder
        {
            ToType = MessageHead.FromType,
            Uid = MessageHead.FromUin,
            GroupCode = MessageHead.GroupInfo?.GroupCode,
            Content = MessageBody?.Content ?? string.Empty,
            AtList = MessageBody?.AtUinLists,
            Images = MessageBody?.Images,
            Voice = MessageBody?.Voice,
        };
    }

    public bool TryGetImages([NotNullWhen(true)] out IList<string>? images)
    {
        images = MessageBody?.Images?
                            .Where(x => !string.IsNullOrEmpty(x.Url))
                            .Select(x => x.Url ?? string.Empty).ToList();
        return images is not null;
    }

    public bool TryGetVideo([NotNullWhen(true)] out string video)
    {
        // TODO 实现TryGetVideo
        video = string.Empty;
        return MessageBody?.Video is not null;
    }

    public bool TryGetVoice([NotNullWhen(true)] out string voice)
    {
        // TODO 实现TryGetVoice
        voice = string.Empty;
        return MessageBody?.Voice is not null;
    }
}

