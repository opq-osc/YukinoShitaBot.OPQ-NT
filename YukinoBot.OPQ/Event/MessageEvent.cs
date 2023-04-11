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

    public string Content => GenerateContent();

    // TODO 多模态Content支持
    private string GenerateContent()
    {
        // 好友消息，对方正在输入
        if (MessageHead.MsgType == 528) return "$event:inputing";
        // 好友消息，视频图片或文字消息
        if (MessageHead.MsgType == 166) return $"{MessageBody!.Content} [$media:image]";  // TODO 使用随机占位符
        // 好友消息，语音消息
        if (MessageHead.MsgType == 208) return $"[$media:voice]";
        // 群消息，视频图片或文字消息
        if (MessageHead.MsgType == 82) return $"{MessageBody!.Content} [$media:image]";
        return string.Empty;
    }

    private HashSet<long>? atList;
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

    public bool TryGetImages([NotNullWhen(true)] out IList<string>? images)
    {
        images = MessageBody?.Images?
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

