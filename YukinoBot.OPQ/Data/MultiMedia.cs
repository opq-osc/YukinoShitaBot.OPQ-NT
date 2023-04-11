namespace YukinoBot.OPQ.Data;

public class MultiMedia
{
    public string FileMd5 { get; set; } = null!;

    public string? FileToken { get; set; }

    public int FileSize { get; set; }

    public long? FileId { get; set; }

    public string? Url { get; set; }
}