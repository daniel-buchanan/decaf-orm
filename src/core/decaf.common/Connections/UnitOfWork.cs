using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common.Connections;

public sealed class UnitOfWork : IUnitOfWorkExtended
{
    private readonly IConnection connection;
    private readonly ITransactionInternal transaction;
    private readonly Action<Guid> notifyDisposed;
    private readonly ISqlFactory sqlFactory;
    private readonly ILoggerProxy logger;
    private readonly DecafOptions options;
    private readonly IHashProvider hashProvider;
    private readonly List<IQueryContainer> queries;
    private Func<Exception, bool>? catchHandler;
    private Action? successHandler;

    private UnitOfWork(
        ITransaction transaction,
        ISqlFactory sqlFactory,
        ILoggerProxy logger,
        IHashProvider hashProvider,
        DecafOptions options,
        Action<Guid> notifyDisposed)
    {
        connection = transaction.Connection;
        this.transaction = (transaction as ITransactionInternal)!;
        this.sqlFactory = sqlFactory;
        this.logger = logger;
        this.options = options;
        this.hashProvider = hashProvider;
        this.notifyDisposed = notifyDisposed;
        queries = new List<IQueryContainer>();
        catchHandler = _ => true;

        Id = Guid.NewGuid();
        this.logger.Debug($"UnitOfWork({Id}) :: Created");
        if(options.SwallowCommitExceptions)
            logger.Warning($"UnitOfWork({Id}) :: Exceptions will be swallowed!");
    }

    /// <inheritdoc />
    public Guid Id { get; private set; }

    /// <inheritdoc />
    IConnection IUnitOfWorkExtended.Connection => connection;

    /// <inheritdoc />
    ITransaction IUnitOfWorkExtended.Transaction => transaction;

    /// <inheritdoc />
    ISqlFactory IUnitOfWorkExtended.SqlFactory => sqlFactory;

    public static IUnitOfWork Create(
        ITransaction transaction,
        ISqlFactory sqlFactory,
        ILoggerProxy logger,
        IHashProvider hashProvider,
        DecafOptions options,
        Action<Guid> notifyDisposed)
        => new UnitOfWork(transaction, sqlFactory, logger, hashProvider, options, notifyDisposed);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;

        if (transaction.State == TransactionState.Disposed ||
            transaction.State == TransactionState.RolledBack)
        {
            logger.Warning($"UnitOfWork({Id}) :: Unable to commit, transaction is already in {transaction.State} state.");
            return;
        }

        Persist();

        logger.Debug($"UnitOfWork({Id}) :: Disposed");
        notifyDisposed(Id);
    }

    private void Persist()
    {
        if (queries.Any(q => q.Status != QueryStatus.Executed))
        {
            logger.Warning($"UnitOfWork({Id}) :: One or more queries have not been executed.");
        }
            
        try
        {
            logger.Debug($"UnitOfWork({Id}) :: Committing Transaction");
            transaction.Commit();
            successHandler?.Invoke();
        }
        catch (Exception commitException)
        {
            logger.Error(commitException, $"UnitOfWork({Id}) :: Committing Transaction Failed");
            var reThrow = catchHandler?.Invoke(commitException);
            try
            {
                logger.Debug($"UnitOfWork({Id}) :: Rolling back Transaction");
                transaction.Rollback();
                if (reThrow is true && !options.SwallowCommitExceptions) throw;
            }
            catch (Exception rollbackException)
            {
                logger.Error(rollbackException, $"UnitOfWork({Id}) :: Rolling back Transaction Failed");
                reThrow = catchHandler?.Invoke(rollbackException);
                if (reThrow is true && !options.SwallowCommitExceptions) throw;
            }
        }
        finally
        {
            if (transaction.CloseConnectionOnCommitOrRollback)
            {
                logger.Debug($"UnitOfWork({Id}) :: Closing Connection after Commit or Rollback");
                connection.Close();
            }
        }
    }

    /// <inheritdoc />
    public IQueryContainer GetQuery()
        => GetQueryAsync().WaitFor();

    /// <inheritdoc />
    public IUnitOfWork Query(Action<IQueryContainer> method)
    {
        var query = GetQuery();
        try
        {
            method(query);
            successHandler?.Invoke();
        } 
        catch (Exception e)
        {
            var reThrow = catchHandler?.Invoke(e);
            if (reThrow is true && !options.SwallowCommitExceptions) throw;
        }

        return this;
    }

    /// <inheritdoc />
    public Task<IQueryContainer> GetQueryAsync(CancellationToken cancellationToken = default)
    {
        transaction.Begin();
            
        var query = QueryContainer.Create(this, logger, hashProvider, options);
        logger.Debug($"UnitOfWork({Id}) :: Creating new Query");
        queries.Add(query);
        return Task.FromResult(query);
    }

    /// <inheritdoc />
    public async Task<IUnitOfWork> QueryAsync(Func<IQueryContainer, Task> method, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryAsync(cancellationToken);
        try
        {
            await method(query);
            successHandler?.Invoke();
        }
        catch (Exception e)
        {
            var reThrow = catchHandler?.Invoke(e);
            if (reThrow == true && !options.SwallowCommitExceptions) throw;
        }

        return this;
    }

    /// <inheritdoc />
    public IUnitOfWork OnException(Action<Exception> handler) 
        => OnException(e =>
        {
            handler(e);
            return false;
        });

    /// <inheritdoc />
    public IUnitOfWork OnException(Func<Exception, bool> handler)
    {
        catchHandler = handler;
        return this;
    }

    /// <inheritdoc />
    public async Task<IUnitOfWork> OnExceptionAsync(Func<Exception, Task> handler, CancellationToken cancellationToken = default) 
        => await OnExceptionAsync(async e =>
        {
            await handler(e);
            return true;
        }, cancellationToken);

    /// <inheritdoc />
    public Task<IUnitOfWork> OnExceptionAsync(Func<Exception, Task<bool>> handler, CancellationToken cancellationToken = default)
    {
        catchHandler = ex => handler(ex).WaitFor();
        return Task.FromResult<IUnitOfWork>(this);
    }

    /// <inheritdoc />
    public IUnitOfWork OnSuccess(Action handler)
    {
        return OnSuccessAsync(() =>
        {
            handler();
            return Task.CompletedTask;
        }).WaitFor();
    }

    /// <inheritdoc />
    public Task<IUnitOfWork> OnSuccessAsync(Func<Task> handler, CancellationToken cancellationToken = default)
    {
        successHandler = () => handler().WaitFor(cancellationToken);
        var uow = this as IUnitOfWork;
        return Task.FromResult(uow);
    }

    /// <inheritdoc />
    public IUnitOfWork PersistChanges() 
        => PersistChangesAsync().WaitFor();

    /// <inheritdoc />
    public Task<IUnitOfWork> PersistChangesAsync(CancellationToken cancellationToken = default)
    {
        Persist();
        var uow = this as IUnitOfWork;
        return Task.FromResult(uow);
    }

    /// <inheritdoc />
    public void NotifyQueryDisposed(Guid queryId)
    {
        var found = queries.FirstOrDefault(q => q.Id == queryId);
        if (found == null)
        {
            logger.Debug($"UnitOfWork({Id}) :: Could not find query with Id - {queryId}");
            return;
        }

        queries.Remove(found);

        if (queries.Count != 0) return;

        Dispose();
    }
}