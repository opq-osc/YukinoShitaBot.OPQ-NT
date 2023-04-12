using YukinoBot.Abstraction;
using YukinoBot.Entity;

namespace YukinoOPQ
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IMessageReceiver messageReceiver;
        private readonly IMessageSender messageSender;
        private readonly IMessageBuilderFactory msgBuilder;

        public Worker(ILogger<Worker> logger, IMessageReceiver messageReceiver, IMessageSender messageSender, IMessageBuilderFactory msgBuilder)
        {
            this.logger = logger;
            this.messageReceiver = messageReceiver;

            this.messageReceiver.OnMessage += OnMessage;
            messageReceiver.OnEvent += OnEvent;

            this.messageSender = messageSender;
            this.msgBuilder = msgBuilder;
        }

        private void OnEvent(object? sender, IEvent e)
        {
            
        }

        private void OnMessage(object? sender, IMessage msg)
        {
            logger.LogInformation("Message received from: {Nick}\n{Msg}", msg.From.GetUserName(), msg.Content);
            if (msg.AtUsers.Any(x => x.GetUserId() == "1137361788") || msg.Content.StartsWith("repeat"))
            {
                var retMsg = msgBuilder.CreateReply(msg)
                                       .WithContent(msg.Content)
                                       .Build();
                messageSender.Send(retMsg);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await messageReceiver.Start();
            logger.LogInformation("YukinoBot started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            messageReceiver.Stop();
        }
    }
}