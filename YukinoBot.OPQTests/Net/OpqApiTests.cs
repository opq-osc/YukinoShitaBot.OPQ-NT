using Microsoft.VisualStudio.TestTools.UnitTesting;
using YukinoBot.OPQ.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YukinoBot.OPQ.Message;

namespace YukinoBot.OPQ.Net.Tests
{
    [TestClass()]
    public class OpqApiTests
    {
        [TestMethod()]
        public void SendTextTest()
        {
            var msgBuilder = new MessageBuilder();
            var api = new OpqApi(new Configuration.OpqOptions
            {
                Host = "ng.baidu.com:8086",
                Uin = 501604732
            });
            var msg1 = msgBuilder.CreateGroup(689857269)
                                .WithContent("test测试123")
                                .At(1137361788)
                                .Build();

            var msg2 = msgBuilder.CreateFriend(1137361788)
                                 .WithContent("test测试123")
                                 .Build();
            var msg3 = msgBuilder.CreateSession(1137361788, 689857269)
                                 .WithContent("test测试123")
                                 .Build();
            api.Send(msg1);
            Thread.Sleep(1000);
            api.Send(msg2);
            Thread.Sleep(1000);
            api.Send(msg3);
        }
    }
}