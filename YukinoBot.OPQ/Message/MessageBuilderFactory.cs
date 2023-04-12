using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;

namespace YukinoBot.OPQ.Message;

public class MessageBuilderFactory : IMessageBuilderFactory
{
    public MessageBuilderFactory(OpqOptions options)
    {
        this.options = options;
    }

    private readonly OpqOptions options;

    public IMessageBuilder Create()
    {
        var builder = new MessageBuilder();
        builder.Sender = new Entity.User
        {
            SenderId = options.Uin,
            Nick = string.Empty
        };
        return builder;
    }

    public IMessageBuilder CreateReply(IMessage message)
    {
        if (message is not Entity.Message msg) throw new Exception();
        var builder = new MessageBuilder();
        builder.Sender = new Entity.User
        {
            SenderId = options.Uin,
            Nick = string.Empty
        };
        builder.Receiver = new Entity.User
        {
            SenderId = msg.Source switch
            {
                "Group" => 0L,
                _ => msg.From.SenderId
            },
            Nick = msg.From.Nick,
            GroupId = msg.From.GroupId,
        };

        return builder;
    }
}
