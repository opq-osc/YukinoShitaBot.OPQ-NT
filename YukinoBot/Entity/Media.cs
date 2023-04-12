using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class Media : IMedia
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string? Url { get; set; }

    public FileInfo? File { get; set; }

    public Guid GetMediaId() => Id;
}
