using YukinoBot;
using YukinoBot.OPQ;

namespace YukinoOPQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddYukinoBot(opt => opt.UseOpqBot(opt =>
                    {
                        opt.Host = "ng.baidu.com:8086";
                        opt.Uin = 1137361788;
                    }));
                })
                .Build();

            host.Run();
        }
    }
}