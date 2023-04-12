using YukinoBot.Entity;

namespace YukinoBot.OPQ.Data;

public class MultiMedia : Media
{
    public string FileMd5 { get; set; } = null!;

    public string? FileToken { get; set; }

    public int FileSize { get; set; }

    public long? FileId { get; set; }
}