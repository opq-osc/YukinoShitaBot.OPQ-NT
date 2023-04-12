using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;
using YukinoBot.OPQ.Event;
using YukinoBot.OPQ.Event.Base;

namespace YukinoBot.OPQ.Net;

public class OpqClient : IMessageReceiver
{
    public OpqClient(OpqOptions options, ILogger<OpqClient> logger)
    {
        cancelationSource = new CancellationTokenSource();
        url = $"ws://{options.Host}/ws";
        uin = options.Uin;
        this.logger = logger;
    }

    private ClientWebSocket? socket;
    private readonly CancellationTokenSource cancelationSource;
    private readonly string url;
    private Thread? workerThread;
    private readonly long uin;
    private readonly ILogger logger;


    public event EventHandler<IInMessage>? OnMessage;
    public event EventHandler<IEvent>? OnEvent;

    public async Task Start()
    {
        socket?.Dispose();
        this.socket = new ClientWebSocket();
        socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(1);
        socket.Options.HttpVersion = HttpVersion.Version11;
        socket.Options.HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact;

        try
        {
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
        catch (Exception ex)
        {
            logger.LogWarning("Exception while starting socket: {Message}", ex.Message);
            logger.LogWarning("Reconnect after 10s");
            await Task.Delay(10000);
            await Start();
        }
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
            if (socket?.State != WebSocketState.Open) break;
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

                    // 过滤非当前绑定QQ号消息
                    if (socketEvent?.CurrentQQ != uin) return;

                    HandleEvent(socketEvent);
                });
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is WebSocketException) break;
                else
                {
                    Console.WriteLine(ex?.InnerException?.Message);
                }
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
                DeSerializeMessage(socketEvent.CurrentPacket.EventData);
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

    // 需要处理的消息类型
    private readonly HashSet<int> acceptMsgTypes = new()
        {
            166,    // 好友视频图片或文字消息
            208,    // 好友语音
            82,     // 群消息
            141,    // 私聊图片或文字消息，或系统通知。需判断MessageBody是否为null
            
        };

    // 已知但无需处理的消息类型
    private readonly HashSet<int> recognizedMsgTypes = new()
        {
            528,    // 好友正在输入，也有其他未知情况会出现
            732,    // 消息已读
            528,    // 对方领取红包
        };

    private void DeSerializeMessage(JsonObject json)
    {
        var msgType = json["MsgHead"]?["MsgType"]?.GetValue<int>() ?? -1;
        if (msgType == -1) return;

        if (!acceptMsgTypes.Contains(msgType))
        {
            if (!recognizedMsgTypes.Contains(msgType))
            {
                logger.LogWarning("未识别消息类型: {Type}", msgType);
                File.WriteAllText(Guid.NewGuid().ToString() + ".json", json.ToJsonString());
            }
            return;
        }

        var sender = json["MsgHead"]?["SenderUin"]?.GetValue<long>();
        if (sender == null || sender == uin) return;

        var msg = json.Root.Deserialize<MessageEvent>();
        if (msg == null) return;

        SetContent(msg);
        if (msg.Content == string.Empty) return;

        OnMessage?.Invoke(this, msg);
    }

    private static void SetContent(MessageEvent msg)
    {
        if (msg.AtList.Count > 0)
        {
            foreach (var at in msg.MessageBody!.AtUinLists!)
            {
                if (string.IsNullOrEmpty(at.Nick)) continue;
                msg.MessageBody!.Content = msg.MessageBody!.Content.Replace($"@{at.Nick} ", string.Empty);
                msg.MessageBody!.Content = msg.MessageBody!.Content.Replace($"@{at.Nick}", string.Empty);
            }
            msg.MessageBody!.Content = msg.MessageBody!.Content.Trim(' ');
        }

        if (msg.TryGetVoice(out _)) msg.Content = "[$media:voice]";
        else if (msg.TryGetVideo(out _)) msg.Content = "[$media:video]";
        else if (msg.TryGetImages(out _)) msg.Content = $"[$media:image]{msg.MessageBody?.Content}";
        else msg.Content = msg.MessageBody?.Content ?? string.Empty;
    }
}
