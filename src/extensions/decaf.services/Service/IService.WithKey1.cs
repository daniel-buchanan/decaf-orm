using System;
namespace decaf.services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IService<TEntity, TKey> :
        IQuery<TEntity, TKey>,
        ICommand<TEntity, TKey>
        where TEntity : IEntity<TKey>, new()
    {
        /// <summary>
        /// Event fired before the query is executed.
        /// </summary>
        new event EventHandler<PreExecutionEventArgs> OnBeforeExecution;
    }
}

