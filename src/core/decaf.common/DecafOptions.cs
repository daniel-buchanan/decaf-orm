﻿using decaf.common;
using decaf.common.Connections;
using decaf.Exceptions;
using Microsoft.Extensions.DependencyInjection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.playground")]
namespace decaf;

/// <summary>
/// Set of options for configuring decaf.
/// </summary>
public class DecafOptions
{
    public DecafOptions()
    {
        DefaultLogLevel = LogLevel.Error;
        DefaultClauseHandling = ClauseHandling.And;
        TrackUnitsOfWork = false;
        CloseConnectionOnCommitOrRollback = false;
        IncludeHeaderCommentsInSql = true;
    }

    /// <summary>
    /// The default log level to use, this is set to <see cref="LogLevel.Error"/>
    /// by default, unless changed.
    /// </summary>
    public LogLevel DefaultLogLevel { get; private set; }

    /// <summary>
    /// The default where clause handling behaviour, this is set to <see cref="ClauseHandling.And"/>.
    /// If you want to ensure that you always set this explicitly, override with <see cref="ClauseHandling.Unspecified"/> and
    /// the <see cref="WhereBuildFailedException"/> will be thrown if you haven't set it.
    /// </summary>
    public ClauseHandling DefaultClauseHandling { get; private set; }

    /// <summary>
    /// Whether or not to track transients as they are used, and disposed.
    /// </summary>
    public bool TrackUnitsOfWork { get; private set; }

    /// <summary>
    /// Whether or not to close the connection on commit or rollback of the transaction.
    /// </summary>
    public bool CloseConnectionOnCommitOrRollback { get; private set; }

    /// <summary>
    /// Whether or not to include header comments in generated SQL.
    /// </summary>
    public bool IncludeHeaderCommentsInSql { get; private set; }
        
    /// <summary>
    /// Whether or not to automatically inject an <see cref="IUnitOfWork"/> as a service.
    /// </summary>
    public bool InjectUnitOfWork { get; private set; }
        
    /// <summary>
    /// Lazily initialise database connections, rather than eagerly opening them before they are used.
    /// </summary>
    public bool LazyInitialiseConnections { get; private set; }
        
    /// <summary>
    /// The lifetime for the <see cref="IUnitOfWork"/> when injected.
    /// </summary>
    public ServiceLifetime UnitOfWorkLifetime { get; private set; }
        
    /// <summary>
    /// Swallow exceptions thrown while committing or rolling back a transaction. NOTE: This is not recommended.
    /// </summary>
    public bool SwallowCommitExceptions { get; private set; }
}