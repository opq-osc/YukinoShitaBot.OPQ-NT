using System.Text.Json.Nodes;

namespace YukinoBot.OPQ.Data;

public class MessageHead
{
    public long FromUin { get; set; }
    public long ToUin { get; set; }
    public int FromType { get; set; }
    public long SenderUin { get; set; }
    public string SenderNick { get; set; } = null!;
    
    public int C2cCmd { get; set; }

    public int MsgType { get; set; }
    public int MsgSeq { get; set; }
    public int MsgTime { get; set; }
    public int MsgRandom { get; set; }
    public long MsgUid { get; set; }

    public JsonObject? C2CTempMessageHead { get; set; }
    public GroupInfo? GroupInfo { get; set; }
}

