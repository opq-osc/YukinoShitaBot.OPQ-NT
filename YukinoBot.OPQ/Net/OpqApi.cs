using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using YukinoBot.Abstraction;
using YukinoBot.OPQ.Configuration;

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

        public void Send(IOutMessage message)
        {
            var content = message.Serialize();
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            httpClient.Send(req);
        }
    }
}
