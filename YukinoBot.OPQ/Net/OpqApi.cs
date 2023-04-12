using System.Text;
using System.Text.Json;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;
using YukinoBot.OPQ.Data;

namespace YukinoBot.OPQ.Net
{
    public class OpqApi : IMessageSender
    {
        public OpqApi(OpqOptions options)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{options.Host}/v1/LuaApiCaller")
            };
            url = $"?funcname=MagicCgiCmd&timeout=10&qq={options.Uin}";
        }

        private readonly HttpClient httpClient;
        private readonly string url;
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public void Send(OutMessage message)
        {
            var reqObj = new SendMessageRequest()
            {
                CgiRequest = message
            };
            var content = JsonSerializer.Serialize(reqObj, jsonSerializerOptions);
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            httpClient.Send(req);
        }

        public void Send(IMessage message)
        {
            if (message is not Entity.Message msg) return;

            var toUin = msg.To switch
            {
                { SenderId: 0, GroupId: not null } => (long)msg.To.GroupId,
                { SenderId: _, GroupId: not null } => msg.To.SenderId,
                { SenderId: _, GroupId: null } => msg.To.SenderId,
            };
            var sendType = msg.To switch
            {
                { SenderId: 0, GroupId: not null } => 2,
                { SenderId: _, GroupId: not null } => 3,
                { SenderId: _, GroupId: null } => 1,
            };

            var outMsg = new OutMessage()
            {
                ToUin = toUin,
                ToType = sendType,
                AtUinLists = msg.AtUsers.Select(x => new Uin
                {
                    UserId = x.SenderId,
                    Nick = x.Nick
                }).ToList(),
                Content = msg.Content,
                GroupCode = msg.To.GroupId,
                Images = msg.Medias.Where(x => x.Type == "image").Cast<MultiMedia>().ToList(),
                Voice = msg.Medias.Where(x => x.Type =="voice").Cast<MultiMedia>().SingleOrDefault()
            };

            Send(outMsg);
        }
    }
}
