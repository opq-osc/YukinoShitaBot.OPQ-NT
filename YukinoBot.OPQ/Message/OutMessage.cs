using YukinoBot.OPQ.Data;

namespace YukinoBot.OPQ.Message;

public class OutMessage
{
    internal OutMessage() { }

    public long ToUin { get; set; }

    // 1-friend, 2-group, 3-session
    public virtual int ToType { get; set; }

    public long? GroupCode { get; set; }

    public string Content { get; set; } = string.Empty;

    public virtual IList<Uin>? AtUinLists { get; set; }

    public virtual IList<MultiMedia>? Images { get; set; }

    public virtual MultiMedia? Voice { get; set; }
}
