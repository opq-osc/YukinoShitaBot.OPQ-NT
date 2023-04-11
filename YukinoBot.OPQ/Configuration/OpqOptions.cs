using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoBot.OPQ.Configuration
{
    public class OpqOptions
    {
        public long Uin { get; set; }
        public string Host { get; set; } = "127.0.0.1:8086";
    }
}
