namespace YukinoBot.OPQ.Data;

public class MessageBody
{
    public int SubMsgType { get; set; }
    public string Content { get; set; } = null!;
    public IList<Uin>? AtUinLists { get; set; }
    public IList<MultiMedia>? Images { get; set; }
    public MultiMedia? Video { get; set; }
    public MultiMedia? Voice { get; set; }
}

