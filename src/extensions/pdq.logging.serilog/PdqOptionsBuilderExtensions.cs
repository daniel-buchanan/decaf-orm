using Microsoft.Extensions.DependencyInjection;
using pdq.common.Logging;
using pdq.common.Options;
using Serilog;

namespace pdq.logging.serilog
{
    public static class PdqOptionsBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IPdqOptionsBuilder UseSerilog(this IPdqOptionsBuilder self)
        {
            var builder = self as IPdqOptionsBuilderExtensions;
            builder.Services.AddScoped<ILoggerProxy, LoggerProxy>();
            return self;
        }
    }
}