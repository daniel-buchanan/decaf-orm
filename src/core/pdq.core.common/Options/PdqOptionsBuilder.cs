using Microsoft.Extensions.DependencyInjection;
using pdq.common.Logging;
using pdq.common.Options;

namespace pdq.common
{
    public class PdqOptionsBuilder :
        OptionsBuilder<PdqOptions>,
        IPdqOptionsBuilderExtensions
    {
        public PdqOptionsBuilder(IServiceCollection services)
            => Services = services;

        /// <inheritdoc/>
        public IServiceCollection Services { get; private set; }

        /// <inheritdoc/>
        public IPdqOptionsBuilder TrackUnitsOfWork()
            => ConfigureProperty(nameof(PdqOptions.TrackUnitsOfWork), true);

        /// <inheritdoc/>
        public IPdqOptionsBuilder OverrideDefaultClauseHandling(ClauseHandling handling)
            => ConfigureProperty(nameof(PdqOptions.DefaultClauseHandling), handling);

        /// <inheritdoc/>
        public IPdqOptionsBuilder OverrideDefaultLogLevel(LogLevel level)
            => ConfigureProperty(nameof(PdqOptions.DefaultLogLevel), level);

        /// <inheritdoc/>
        public IPdqOptionsBuilder CloseConnectionsOnCommitOrRollback()
            => ConfigureProperty(nameof(PdqOptions.CloseConnectionOnCommitOrRollback), true);

        /// <inheritdoc/>
        public IPdqOptionsBuilder DisableSqlHeaderComments()
            => ConfigureProperty(nameof(PdqOptions.IncludeHeaderCommentsInSql), false);

        /// <inheritdoc/>
        public IPdqOptionsBuilder InjectUnitOfWorkAsScoped()
            => InjectUnitOfWork(ServiceLifetime.Scoped);

        /// <inheritdoc/>
        public IPdqOptionsBuilder InjectUnitOfWork(ServiceLifetime lifetime)
        {
            ConfigureProperty(nameof(PdqOptions.InjectUnitOfWork), true);
            ConfigureProperty(nameof(PdqOptions.UnitOfWorkLifetime), lifetime);
            return this;
        }
        
        internal IPdqOptionsBuilder SetLoggerProxy<T>() where T : ILoggerProxy
            => ConfigureProperty(nameof(PdqOptions.LoggerProxyType), typeof(T));

        private new IPdqOptionsBuilder ConfigureProperty<TValue>(string name, TValue value)
        {
            base.ConfigureProperty(name, value);
            return this;
        }
    }
}

