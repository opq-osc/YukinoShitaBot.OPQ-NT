using YukinoBot.Abstraction;

namespace YukinoOPQ
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IMessageReceiver messageReceiver;
        private readonly IMessageSender messageSender;

        public Worker(ILogger<Worker> logger, IMessageReceiver messageReceiver, IMessageSender messageSender)
        {
            this.logger = logger;
            this.messageReceiver = messageReceiver;

            messageReceiver.OnMessage += OnMessage;
            messageReceiver.OnEvent += OnEvent;
            this.messageSender = messageSender;
        }

        private void OnEvent(object? sender, IEvent e)
        {
            
        }

        private void OnMessage(object? sender, IInMessage e)
        {
            logger.LogInformation("Message received: {Msg}", e.Content);
            if (e.AtList.Contains(1137361788) || e.Content.StartsWith("repeat"))
            {
                var msg = e.CreateRepeat()
                           .Build();
                messageSender.Send(msg);
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