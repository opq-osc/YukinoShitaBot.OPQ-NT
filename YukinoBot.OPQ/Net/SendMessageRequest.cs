using YukinoBot.OPQ.Message;

namespace YukinoBot.OPQ.Net;

public class SendMessageRequest : OpqRequest<OutMessage>
{
    public override string CgiCmd => "MessageSvc.PbSendMsg";
}

