using decaf.common.Logging;
using decaf.common.Options;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.logging.serilog
{
    public static class DecafOptionsBuilderExtensions
    {
        /// <summary>
        /// Use Serilog as the logger for decaf.
        /// </summary>
        /// <param name="self"></param>
        /// <returns>(FluentAPI) an <see cref="IDecafOptionsBuilder"/></returns>
        public static IDecafOptionsBuilder UseSerilog(this IDecafOptionsBuilder self)
        {
            var builder = self as IDecafOptionsBuilderExtensions;
            builder.Services.AddScoped<ILoggerProxy, LoggerProxy>();
            return self;
        }
    }
}