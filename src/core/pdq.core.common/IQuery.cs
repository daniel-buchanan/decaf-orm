using System;

namespace pdq.common
{
	/// <summary>
    /// <see cref="IQuery"/> represents an individual query that should be executed.
    /// It may be one of the following kinds:<br/>
    /// - Select<br/>
    /// - Delete<br/>
    /// - Update<br/>
    /// - Insert
    /// </summary>
	public interface IQuery : IDisposable
	{
        /// <summary>
        /// The Id of the query, this is uniquely generated for each query and
        /// cannot be changed.
        /// </summary>
		Guid Id { get; }

        /// <summary>
        /// The status of the query, this will change as it is built and/or executed.
        /// </summary>
		QueryStatus Status { get; }
	}
}

