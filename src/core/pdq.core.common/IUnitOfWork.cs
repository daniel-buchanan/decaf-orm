using System.Threading.Tasks;

namespace pdq.common
{
	/// <summary>
    /// The Unit of Work provides a wrapper around what is effectively the
    /// "Transient" factory. this means that you can generate a "Transient",
    /// execute queries on it and then dispose of it (and have the transaction
    /// commit/rollback dealt with for you).
    /// </summary>
	public interface IUnitOfWork
	{
        /// <summary>
        /// Begin a Transient on the Unit of Work. This will create a Transient
        /// and underlying transaction for your queries to run on.
        /// </summary>
        /// <returns>
        /// (FluentApi) A Transient instance that you can use to create queries
        /// and dispose of when you are finished.
        /// </returns>
        /// <example>
        /// using (var t = uow.Begin()) {
        ///    var q = t.Query();
        ///    ...
        /// }
        /// </example>
        ITransient Begin();

        /// <summary>
        /// Begin a Transient on the Unit of Work. This will create a Transient
        /// and underlying transaction for your queries to run on.
        /// </summary>
        /// <returns>
        /// (FluentApi) A Transient instance that you can use to create queries
        /// and dispose of when you are finished.
        /// </returns>
        /// <example>
        /// using (var t = await uow.BeginAsync()) {
        ///    var q = t.Query();
        ///    ...
        /// }
        /// </example>
        Task<ITransient> BeginAsync();
	}
}

