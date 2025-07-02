using System;
using System.Data;
using decaf.common.Exceptions;
using decaf.common.Logging;

namespace decaf.common.Connections;

public abstract class Transaction : ITransactionInternal
{
    private readonly ILoggerProxy logger;
    private readonly DecafOptions options;
    private IDbTransaction? transaction;

    protected Transaction(
        Guid id,
        ILoggerProxy logger,
        IConnection connection,
        DecafOptions options)
    {
        Id = id;
        this.logger = logger;
        this.options = options;
        Connection = connection;
        State = TransactionState.Created;

        this.logger.Debug($"ITransaction({Id}) :: Transaction created");
    }

    /// <inheritdoc/>
    public IConnection Connection { get; protected set; }

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    bool ITransactionInternal.CloseConnectionOnCommitOrRollback => options.CloseConnectionOnCommitOrRollback;

    /// <inheritdoc/>
    public void Begin()
    {
        if (transaction != null) return;

        logger.Debug($"ITransaction({Id}) :: Beginning Transaction");
        if(!options.LazyInitialiseConnections)
            transaction = GetUnderlyingTransaction();
        State = TransactionState.Begun;
    }

    /// <inheritdoc/>
    public void Commit()
    {
        try
        {
            if (options.LazyInitialiseConnections)
                Begin();
                
            logger.Debug($"ITransaction({Id}) :: Committing Transaction");
            transaction?.Commit();
            State = TransactionState.Committed;
            logger.Debug($"ITransaction({Id}) :: Commit SUCCEEDED");
        }
        catch (Exception commitEx)
        {
            try
            {
                var ex = new CommitException(commitEx,
                    $"Transaction {Id} Commit Failed. See inner exception for more information.");
                logger.Error(commitEx, $"ITransaction({Id}) :: Commit FAILED, attempting Rollback");
                State = TransactionState.CommitFailed;
                RollbackTransaction();
                throw ex;
            }
            catch (Exception rollbackEx)
            {
                if (rollbackEx is CommitException)
                    throw;
                    
                var ex = new RollbackException(rollbackEx,
                    $"Transaction {Id} Rollback Failed. See inner exception for more information.");
                logger.Error(rollbackEx, $"ITransaction({Id}) :: Rollback FAILED");
                State = TransactionState.RollbackFailed;
                throw ex;
            }
        }
        finally
        {
            logger.Debug($"ITransaction({Id}) :: Finished Commit Process");
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        transaction = null;
        State = TransactionState.Disposed;
    }

    private void RollbackTransaction()
    {
        logger.Debug($"ITransaction({Id}) :: Rolling back Transaction");
        transaction?.Rollback();
        logger.Debug($"ITransaction({Id}) :: Rollback SUCCEEDED");
        State = TransactionState.RolledBack;
    }
        
    /// <inheritdoc/>
    public void Rollback()
    {
        try
        {
            RollbackTransaction();
            State = TransactionState.RolledBack;
        }
        catch (Exception rollbackEx)
        {
            var ex = new RollbackException(rollbackEx,
                $"Transaction {Id} Rollback Failed. See inner exception for more information.");
            logger.Error(rollbackEx, $"ITransaction({Id}) :: Rollback FAILED");
            State = TransactionState.RollbackFailed;
            throw ex;
        }
    }

    /// <inheritdoc/>
    public abstract IDbTransaction GetUnderlyingTransaction();

    /// <inheritdoc/>
    public TransactionState State { get; private set; }
}