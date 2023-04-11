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
        public OpqClient(OpqOptions options)
        {
            cancelationSource = new CancellationTokenSource();
            this.socket = new ClientWebSocket();
            socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            url = $"ws://{options.Host}/ws";
        }

        private readonly ClientWebSocket socket;
        private readonly CancellationTokenSource cancelationSource;
        private readonly string url;
        private Thread? workerThread;

        public async Task Start()
        {
            await socket.ConnectAsync(new Uri(url), cancelationSource.Token);

            while (socket.State == WebSocketState.Connecting) Thread.SpinWait(1000);
            if (socket.State != WebSocketState.Open)
            {
                throw new ApplicationException("socket connect failed.");
            }

            workerThread = new Thread(Receive)
            {
                IsBackground = true
            };
            workerThread.Start();
        }

        private void Receive()
        {
            var buffer = new byte[8192];
            while (true)
            {
                if (socket.State != WebSocketState.Open) break;
                try
                {
                    var result = socket.ReceiveAsync(buffer, cancelationSource.Token).Result;
                    if (!result.EndOfMessage)
                    {
                        throw new ApplicationException("socket buffer overflow");
                    }
                    var span = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                    var socketEvent = JsonSerializer.Deserialize<SocketEvent>(span);

                    if (socketEvent is null) continue;
                    HandleEvent(socketEvent);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                catch (JsonException)
                {
                    continue;
                }
            }
            Start().Wait();
        }

        // TODO HandleEvent
        private void HandleEvent(SocketEvent socketEvent)
        {
            //if (socketEvent.CurrentPacket.EventName == "ON_EVENT_GROUP_NEW_MSG")
            //{
            //    var msg = socketEvent.CurrentPacket.EventData.Root.Deserialize<MessageEvent>();
                
            //}
        }

        public void OnEvent<T>(T message) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public void OnMessage<T>(T message) where T : IInMessage
        {
            throw new NotImplementedException();
        }
    }
}
