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
            self.ConfigureProperty(o => o.LoggerProxyType, typeof(LoggerProxy));
            return self;
        }
    }
}