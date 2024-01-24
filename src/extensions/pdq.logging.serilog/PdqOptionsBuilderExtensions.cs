using Microsoft.Extensions.DependencyInjection;
using pdq.common.Logging;
using pdq.common.Options;

namespace pdq.logging.serilog
{
    public static class PdqOptionsBuilderExtensions
    {
        /// <summary>
        /// Use Serilog as the logger for pdq.
        /// </summary>
        /// <param name="self"></param>
        /// <returns>(FluentAPI) an <see cref="IPdqOptionsBuilder"/></returns>
        public static IPdqOptionsBuilder UseSerilog(this IPdqOptionsBuilder self)
        {
            var builder = self as IPdqOptionsBuilderExtensions;
            builder.Services.AddScoped<ILoggerProxy, LoggerProxy>();
            return self;
        }
    }
}