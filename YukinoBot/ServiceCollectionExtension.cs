using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoBot
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddYukinoBot(this IServiceCollection services, Action<YukinoBotOptions> configOptions)
        {
            var opt = new YukinoBotOptions(services);
            configOptions(opt);

            // TODO 配置Controller、Services、View

            return services;
        }
    }
}
