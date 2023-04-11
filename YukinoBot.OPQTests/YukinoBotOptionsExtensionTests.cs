using Microsoft.VisualStudio.TestTools.UnitTesting;
using YukinoBot.OPQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Tests
{
    [TestClass()]
    public class YukinoBotOptionsExtensionTests
    {
        [TestMethod()]
        public void UseOpqBotTest()
        {
            var services = new ServiceCollection();

            services.AddYukinoBot(opt => opt.UseOpqBot(opt =>
            {
                opt.Host = "ng.baidu.com:8086";
                opt.Uin = 501604732;
            }));

            var provider = services.BuildServiceProvider();

            var sender = provider.GetService<IMessageSender>();
            Assert.IsNotNull(sender);

            var builder = provider.GetService<IMessageBuilder>();
            Assert.IsNotNull(builder);

            var receiver = provider.GetService<IMessageReceiver>();
            Assert.IsNotNull(receiver);

            var msg = builder.CreateFriend(1137361788).WithContent("test").Build();
            sender.Send(msg);
        }
    }
}