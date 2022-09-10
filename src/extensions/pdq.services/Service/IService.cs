using System;
namespace pdq.services
{
	/// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
	public interface IService<TEntity> :
		IQuery<TEntity>,
		ICommand<TEntity>
		where TEntity : IEntity, new()
	{
        /// <summary>
        /// Event fired before the query is executed.
        /// </summary>
        new event EventHandler<PreExecutionEventArgs> PreExecution;
    }
}

