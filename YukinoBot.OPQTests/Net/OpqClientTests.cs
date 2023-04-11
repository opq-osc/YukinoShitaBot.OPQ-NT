using Microsoft.VisualStudio.TestTools.UnitTesting;
using YukinoBot.OPQ.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoBot.OPQ.Net.Tests
{
    [TestClass()]
    public class OpqClientTests
    {
        [TestMethod()]
        public void StartTest()
        {
            var client = new OpqClient();
            client.Start().Wait();
            while (true) ;
        }
    }
}