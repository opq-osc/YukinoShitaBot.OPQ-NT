using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;
using YukinoBot.OPQ.Event;
using YukinoBot.OPQ.Event.Base;

namespace YukinoBot.OPQ.Net
{
    public class OpqClient : IMessageReceiver
    {
        public OpqClient(OpqOptions options, ILogger<OpqClient> logger)
        {
            cancelationSource = new CancellationTokenSource();
            this.socket = new ClientWebSocket();
            socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(1);
            socket.Options.HttpVersion = HttpVersion.Version11;
            socket.Options.HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact;
            url = $"ws://{options.Host}/ws";
            uin = options.Uin;
            this.logger = logger;
        }

        private readonly ClientWebSocket socket;
        private readonly CancellationTokenSource cancelationSource;
        private readonly string url;
        private Thread? workerThread;
        private readonly long uin;
        private readonly ILogger logger;


        public event EventHandler<IInMessage>? OnMessage;
        public event EventHandler<IEvent>? OnEvent;

        public async Task Start()
        {
            if (socket.State == WebSocketState.Open)
            {
                socket.Abort();
                logger.LogInformation("socket closed.");
            }
            await socket.ConnectAsync(new Uri(url), cancelationSource.Token);

            while (socket.State == WebSocketState.Connecting) Thread.SpinWait(1000);
            if (socket.State != WebSocketState.Open)
            {
                throw new ApplicationException("socket connect failed.");
            }
            logger.LogInformation("socket connected.");

            workerThread = new Thread(Receive)
            {
                IsBackground = true
            };
            workerThread.Start();
        }

        public void Stop()
        {
            cancelationSource.Cancel();
        }

        private void Receive()
        {
            var buffer = new byte[8192];
            while (!cancelationSource.IsCancellationRequested)
            {
                if (socket.State != WebSocketState.Open) break;
                try
                {
                    var result = socket.ReceiveAsync(buffer, cancelationSource.Token).Result;
                    // TODO 考虑是否需要拼帧
                    if (!result.EndOfMessage)
                    {
                        throw new ApplicationException("socket buffer overflow");
                    }
                    Task.Run(() =>
                    {
                        var span = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                        var socketEvent = JsonSerializer.Deserialize<SocketEvent>(span);

                        if (socketEvent is null) return;
                        HandleEvent(socketEvent);
                    });
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException is WebSocketException) break;
                }
            }
            Start().Wait();
        }

        // TODO HandleEvent
        private void HandleEvent(SocketEvent socketEvent)
        {
            switch (socketEvent.CurrentPacket.EventName)
            {
                case "ON_EVENT_FRIEND_NEW_MSG" or "ON_EVENT_GROUP_NEW_MSG":
                    var msg = socketEvent.CurrentPacket.EventData.Root.Deserialize<MessageEvent>();
                    if (msg is null || msg.MessageHead.SenderUin == uin) return;
                    OnMessage?.Invoke(this, msg);
                    break;
                case "ON_EVENT_LOGIN_SUCCESS":
                    var eventLogin = socketEvent.CurrentPacket.EventData.Root.Deserialize<LoginEvent>();
                    if (eventLogin is null) return;
                    OnEvent?.Invoke(this, eventLogin);
                    break;
                case "ON_EVENT_NETWORK_CHANGE":
                    var eventNetwork = socketEvent.CurrentPacket.EventData.Root.Deserialize<LoginEvent>();
                    if (eventNetwork is null) return;
                    OnEvent?.Invoke(this, eventNetwork);
                    break;
            }
        }
    }
}
