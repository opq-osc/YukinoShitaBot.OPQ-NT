namespace YukinoBot.OPQ.Data;

public class GroupInfo
{
    public string GroupCard { get; set; } = null!;
    public long GroupCode { get; set; }
    public int GroupInfoSeq { get; set; }
    public int GroupLevel { get; set; }
    public int GroupRank { get; set; }
    public int GroupType { get; set; }
    public string GroupName { get; set; } = null!;
}

