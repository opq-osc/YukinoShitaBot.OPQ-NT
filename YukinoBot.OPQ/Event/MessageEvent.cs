using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
    public string Content => GenerateContent();

    private string GenerateContent()
    {
        // 好友消息，对方正在输入
        if (MessageHead.MsgType == 528) return "$event:inputing";
        // 好友消息，视频图片或文字消息
        if (MessageHead.MsgType == 166) return $"{MessageBody!.Content} [$media:image]";
        // 好友消息，语音消息
        if (MessageHead.MsgType == 208) return $"[$media:voice]";
        // 群消息
        if (MessageHead.MsgType == 82)
        {
            if (MessageBody?.Images?.Count > 0) return $"{MessageBody!.Content} [$media:image]";
            if (MessageBody?.Video is not null) return $"[$media:video]";
            if (MessageBody?.Voice is not null) return $"[$media:voice]";
            return $"{MessageBody!.Content}";
        }
        // 私聊消息，图片或文字消息
        if (MessageHead.MsgType == 141) return $"{MessageBody!.Content} [$media:image]";

        // 未识别消息
        // TODO 删除临时测试代码
        Console.WriteLine($"未识别MsgType: {MessageHead.MsgType}");
        File.WriteAllText(Guid.NewGuid().ToString() + ".json", JsonSerializer.Serialize(this));
        return string.Empty;
    }

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
        throw new NotImplementedException();
    }

    public bool TryGetVoice([NotNullWhen(true)] out string voice)
    {
        throw new NotImplementedException();
    }
}

