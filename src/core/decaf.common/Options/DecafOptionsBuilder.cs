using decaf.common.Options;
using Microsoft.Extensions.DependencyInjection;

namespace decaf.common
{
    public class DecafOptionsBuilder :
        OptionsBuilder<DecafOptions>,
        IDecafOptionsBuilderExtensions
    {
        public DecafOptionsBuilder(IServiceCollection services)
            => Services = services;

        /// <inheritdoc/>
        public IServiceCollection Services { get; private set; }

        /// <inheritdoc/>
        public IDecafOptionsBuilder TrackUnitsOfWork()
            => ConfigureProperty(nameof(DecafOptions.TrackUnitsOfWork), true);

        /// <inheritdoc/>
        public IDecafOptionsBuilder OverrideDefaultClauseHandling(ClauseHandling handling)
            => ConfigureProperty(nameof(DecafOptions.DefaultClauseHandling), handling);

        /// <inheritdoc/>
        public IDecafOptionsBuilder OverrideDefaultLogLevel(LogLevel level)
            => ConfigureProperty(nameof(DecafOptions.DefaultLogLevel), level);

        /// <inheritdoc/>
        public IDecafOptionsBuilder CloseConnectionsOnCommitOrRollback()
            => ConfigureProperty(nameof(DecafOptions.CloseConnectionOnCommitOrRollback), true);

        /// <inheritdoc/>
        public IDecafOptionsBuilder DisableSqlHeaderComments()
            => ConfigureProperty(nameof(DecafOptions.IncludeHeaderCommentsInSql), false);

        /// <inheritdoc/>
        public IDecafOptionsBuilder InjectUnitOfWorkAsScoped()
            => InjectUnitOfWork(ServiceLifetime.Scoped);

        /// <inheritdoc/>
        public IDecafOptionsBuilder InjectUnitOfWork(ServiceLifetime lifetime)
        {
            ConfigureProperty(nameof(DecafOptions.InjectUnitOfWork), true);
            ConfigureProperty(nameof(DecafOptions.UnitOfWorkLifetime), lifetime);
            return this;
        }

        private new IDecafOptionsBuilder ConfigureProperty<TValue>(string name, TValue value)
        {
            base.ConfigureProperty(name, value);
            return this;
        }
    }
}

