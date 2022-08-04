using System;
using pdq.common;
using pdq.common.Logging;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.playground")]
namespace pdq
{
    /// <summary>
    /// Set of options for configuring pdq.
    /// </summary>
    public class PdqOptions
    {
        public PdqOptions() : this(
            defaultLogLevel: LogLevel.Error,
            defaultClauseHandling: ClauseHandling.And,
            trackTransients: false,
            closeConnectionOnCommitOrRollback: false,
            loggerProxyType: typeof(DefaultLogger))
        {
            
        }

        public PdqOptions(
            LogLevel? defaultLogLevel,
            ClauseHandling? defaultClauseHandling,
            bool? trackTransients,
            bool? closeConnectionOnCommitOrRollback,
            Type loggerProxyType,
            Type sqlFactoryType = null,
            Type transactionFactoryType = null,
            Type connectionFactoryType = null)
        {
            DefaultLogLevel = defaultLogLevel ?? LogLevel.Error;
            DefaultClauseHandling = defaultClauseHandling ?? ClauseHandling.And;
            TrackTransients = trackTransients ?? false;
            CloseConnectionOnCommitOrRollback = closeConnectionOnCommitOrRollback ?? false;
            LoggerProxyType = loggerProxyType ?? typeof(DefaultLogger);
            SqlFactoryType = sqlFactoryType;
            TransactionFactoryType = transactionFactoryType;
            ConnectionFactoryType = connectionFactoryType;
        }

        /// <summary>
        /// The default log level to use, this is set to <see cref="LogLevel.Error"/>
        /// by default, unless changed.
        /// </summary>
        public LogLevel DefaultLogLevel { get; private set; }

        /// <summary>
        /// The default where clause handling behaviour, this is set to <see cref="ClauseHandling.And"/>.
        /// If you want to ensure that you always set this explictly, override with <see cref="ClauseHandling.Unspecified"/> and
        /// the <see cref="pdq.WhereBuildFailedException"/> will be thrown if you haven't set it.
        /// </summary>
        public ClauseHandling DefaultClauseHandling { get; private set; }

		/// <summary>
        /// Whether or not to track transients as they are used, and disposed.
        /// </summary>
		public bool TrackTransients { get; private set; }

        /// <summary>
        /// Whether or not to close the connection on commit or rollback of the transaction.
        /// </summary>
        public bool CloseConnectionOnCommitOrRollback { get; private set; }

        /// <summary>
        /// The type of the logger proxy to use.
        /// </summary>
        internal Type LoggerProxyType { get; private set; }

		/// <summary>
        /// The type of the sql factory to use.
        /// </summary>
		internal Type SqlFactoryType { get; private set; }

        /// <summary>
        /// The type of connection factory to use.
        /// </summary>
        internal Type ConnectionFactoryType { get; private set; }

        /// <summary>
        /// The type of transaction factory to use.
        /// </summary>
        internal Type TransactionFactoryType { get; private set; }
	}
}

