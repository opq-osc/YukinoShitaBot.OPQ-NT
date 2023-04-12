using Microsoft.Extensions.DependencyInjection;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;
using YukinoBot.OPQ.Message;
using YukinoBot.OPQ.Net;

namespace YukinoBot.OPQ;

public static class YukinoBotOptionsExtension
{
    public static YukinoBotOptions UseOpqBot(this YukinoBotOptions botOptions, Action<OpqOptions> configOptions)
    {
        var opt = new OpqOptions();
        var services = botOptions.Services;

        configOptions.Invoke(opt);
        services.AddSingleton(opt);

        services.AddSingleton<IMessageSender, OpqApi>();
        services.AddSingleton<IMessageReceiver, OpqClient>();
        services.AddSingleton<IMessageBuilderFactory, MessageBuilderFactory>();

        return botOptions;
    }
}
