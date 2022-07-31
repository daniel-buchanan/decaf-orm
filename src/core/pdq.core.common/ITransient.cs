using System;
using pdq.common.Connections;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.common
{
	/// <summary>
    /// 
    /// </summary>
	public interface ITransient : IDisposable
	{
		/// <summary>
        /// The Id of the "Transient", this is unqiuely generated for each transient
        /// at the time that it is created and cannot be changed.
        /// </summary>
		Guid Id { get; }

		/// <summary>
        /// Begin a query on this "Transient", this will allow you to choose what
        /// type of query you wish to write and then execute.
        /// </summary>
        /// <returns>(FluentApi) The ability to begin, write and execute a query.</returns>
        /// <example>t.Query()</example>
		IQuery Query();
	}

    internal interface ITransientInternal : ITransient
    {
        /// <summary>
        /// The connection associated with this Transient.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// The transaction associated with this Transient.
        /// </summary>
        ITransaction Transaction { get; }
    }
}