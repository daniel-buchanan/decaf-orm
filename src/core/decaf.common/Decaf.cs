using System.Threading;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.common.Logging;
using decaf.common.Utilities;

namespace decaf.common;

public class Decaf(
    ILoggerProxy logger,
    IUnitOfWorkFactory unitOfWorkFactory,
    IConnectionDetails? injectedConnectionDetails = null)
    : IDecaf
{
    private IUnitOfWork GetUnitOfWork(IConnectionDetails? connectionDetails = null)
        => GetUnitOfWorkAsync(connectionDetails).WaitFor();

    private async Task<IUnitOfWork> GetUnitOfWorkAsync(
        IConnectionDetails? connectionDetails = null,
        CancellationToken cancellationToken = default)
    {
        if (injectedConnectionDetails is null &&
            connectionDetails is null)
        {
            logger.Error("ConnectionDetails could not be found, either not injected or provided to method");
            throw new MissingConnectionDetailsException("ConnectionDetails were not injected or provided. Please ensure that either method is used.");
        }

        var connectionDetailsToUse = connectionDetails ?? injectedConnectionDetails!;

        return await unitOfWorkFactory.CreateAsync(connectionDetailsToUse, cancellationToken);
    }

    /// <inheritdoc/>
    public IUnitOfWork BuildUnit()
        => GetUnitOfWork();

    /// <inheritdoc/>
    public IUnitOfWork BuildUnit(IConnectionDetails connectionDetails)
        => GetUnitOfWork(connectionDetails);

    /// <inheritdoc/>
    public Task<IUnitOfWork> BuildUnitAsync(CancellationToken cancellationToken = default)
        => GetUnitOfWorkAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<IUnitOfWork> BuildUnitAsync(
        IConnectionDetails connectionDetails,
        CancellationToken cancellationToken = default)
        => await GetUnitOfWorkAsync(connectionDetails, cancellationToken);

    /// <inheritdoc/>
    public IQueryContainer Query()
        => GetUnitOfWork().GetQuery();

    /// <inheritdoc/>
    public IQueryContainer Query(IConnectionDetails connectionDetails)
        => GetUnitOfWork(connectionDetails).GetQuery();

    /// <inheritdoc/>
    public async Task<IQueryContainer> QueryAsync(CancellationToken cancellationToken = default)
        => await QueryAsync(null, cancellationToken);

    /// <inheritdoc/>
    public async Task<IQueryContainer> QueryAsync(
        IConnectionDetails? connectionDetails,
        CancellationToken cancellationToken = default)
    {
        var t = await GetUnitOfWorkAsync(connectionDetails, cancellationToken);
        return await t.GetQueryAsync(cancellationToken);
    }
}