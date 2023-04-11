using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoBot
{
    public class YukinoBotOptions
    {
        public YukinoBotOptions(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; init; }
    }
}
