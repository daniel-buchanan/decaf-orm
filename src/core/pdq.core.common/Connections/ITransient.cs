using System;
using pdq.common.Connections;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common.Connections
{
	/// <summary>
    /// 
    /// </summary>
	public interface ITransient : IDisposable
	{
		/// <summary>
        /// Gets the Id of the "Transient", this is unqiuely generated for each transient
        /// at the time that it is created and cannot be changed.
        /// </summary>
		Guid Id { get; }

		/// <summary>
        /// Begin a query on this "Transient", this will allow you to choose what
        /// type of query you wish to write and then execute.
        /// </summary>
        /// <returns>(FluentApi) The ability to begin, write and execute a query.</returns>
        /// <example>t.Query()</example>
		IQueryContainer Query();
	}

    internal interface ITransientInternal : ITransient
    {
        /// <summary>
        /// Gets the connection associated with this Transient.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// Gets the transaction associated with this Transient.
        /// </summary>
        ITransaction Transaction { get; }

        /// <summary>
        /// Gets the SQL Factory for this query.
        /// </summary>
        ISqlFactory SqlFactory { get; }

        /// <summary>
        /// Notify the transient that a query has been disposed.
        /// </summary>
        /// <param name="id">The ID of the query that was disposed.</param>
        void NotifyQueryDisposed(Guid id);
    }
}