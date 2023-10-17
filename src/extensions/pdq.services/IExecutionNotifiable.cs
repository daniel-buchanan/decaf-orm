using System;

namespace pdq.services
{
    /// <summary>
    /// Meta data about a Key for an <see cref="IEntity"/> or <see cref="IEntity{TKey}"/>.
    /// </summary>
    public interface IExecutionNotifiable
    {

        /// <summary>
        /// Event fired before the query is executed.
        /// </summary>
        event EventHandler<PreExecutionEventArgs> OnBeforeExecution;
    }
}