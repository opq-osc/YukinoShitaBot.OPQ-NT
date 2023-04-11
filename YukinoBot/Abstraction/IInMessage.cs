using System.Diagnostics.CodeAnalysis;

namespace YukinoBot.Abstraction;

public interface IInMessage
{
    string Content { get; }
    HashSet<long> AtList { get; }

    bool TryGetImages([NotNullWhen(true)]out IList<string>? images);
    bool TryGetVoice([NotNullWhen(true)]out string? voice);
    bool TryGetVideo([NotNullWhen(true)]out string? video);

    IMessageBuilder CreateReply();
}

