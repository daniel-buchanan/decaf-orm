using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using decaf.common.Exceptions;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common.Connections;

public sealed class UnitOfWorkFactory(
    DecafOptions options,
    ILoggerProxy logger,
    ITransactionFactory transactionFactory,
    ISqlFactory sqlFactory,
    IHashProvider hashProvider,
    IConnectionDetails? providedConnectionDetails = null)
    : IUnitOfWorkFactoryInternal
{
    private readonly List<IUnitOfWork> tracker = [];

    /// <inheritdoc/>
    public IUnitOfWork Create() => CreateAsync().WaitFor();

    /// <inheritdoc/>
    public IUnitOfWork Create(IConnectionDetails connectionDetails) => CreateAsync(connectionDetails).WaitFor();

    /// <inheritdoc/>
    public async Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
    {
        if (providedConnectionDetails == null)
            throw new MissingConnectionDetailsException("No connection details found.");
        return await CreateAsync(providedConnectionDetails, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IUnitOfWork> CreateAsync(IConnectionDetails connectionDetails, CancellationToken cancellationToken = default)
    {
        logger.Debug("UnitOfWorkFactory :: Getting Transaction");
        var transaction = await transactionFactory.GetAsync(connectionDetails, cancellationToken);
        var uow = UnitOfWork.Create(
            transaction,
            sqlFactory,
            logger,
            hashProvider,
            options,
            NotifyUnitOfWorkDisposed);
        logger.Debug($"UnitOfWorkFactory :: UnitOfWork ({uow.Id}) Tracked");

        if(options.TrackUnitsOfWork) tracker.Add(uow);

        return uow;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (!options.TrackUnitsOfWork) return;

        logger.Debug($"UnitOfWorkFactory :: Disposing Resources, {tracker.Count} UnitOfWorks");
        tracker.Clear();
    }

    private void NotifyUnitOfWorkDisposed(Guid id)
    {
        if (!options.TrackUnitsOfWork) return;

        var uow = tracker.FirstOrDefault(t => t.Id == id);
        if (uow == null)
        {
            logger.Debug($"UnitOfWorkFactory :: Notified UnitOfWork ({id}) disposed, but was not tracked");
            return;
        }

        logger.Debug($"UnitOfWorkFactory :: Notified UnitOfWork({id}) disposed, tracking removed");
        tracker.Remove(uow);
    }

    /// <inheritdoc/>
    void IUnitOfWorkFactoryInternal.NotifyUnitOfWorkDisposed(Guid id)
        => NotifyUnitOfWorkDisposed(id);
}