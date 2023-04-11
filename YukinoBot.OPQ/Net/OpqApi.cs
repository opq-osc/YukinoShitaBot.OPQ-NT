using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Net
{
    public class OpqApi : IMessageSender
    {
        public OpqApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("http://ng.baidu.com:8086/v1/LuaApiCaller");
        }

        private readonly HttpClient httpClient;
        private readonly string url = $"?funcname=MagicCgiCmd&timeout=10&qq=501604732";

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
