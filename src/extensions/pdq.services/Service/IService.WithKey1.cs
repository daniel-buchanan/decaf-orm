using System;
namespace pdq.services
{
	/// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
	public interface IService<TEntity, TKey> :
		IQuery<TEntity, TKey>,
		ICommand<TEntity, TKey>
		where TEntity: IEntity<TKey>
    {

    }
}

