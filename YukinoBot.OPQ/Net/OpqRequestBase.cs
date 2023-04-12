namespace YukinoBot.OPQ.Net;

public abstract class OpqRequest<T> where T : class
{
    public abstract string CgiCmd { get; }

    public T CgiRequest { get; set; } = null!;
}

