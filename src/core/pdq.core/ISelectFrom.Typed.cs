using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface ISelectFromTyped<T> :
        IExecute,
        ISelectColumnTyped<T>,
		IJoinTyped<T>
	{
		IGroupByTyped<T> Where(Expression<Func<T, bool>> builder);
	}

    public interface ISelectFromTyped<T1, T2> :
        IExecute,
        IJoinTyped<T1, T2>
    {
        IGroupByTyped<T1, T2> Where(Expression<Func<T1, T2, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3> :
        IExecute,
        IJoinTyped<T1, T2, T3>
    {
        IGroupByTyped<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4> :
        IExecute,
        IJoinTyped<T1, T2, T3, T4>
    {
        IGroupByTyped<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4, T5> : IExecute
    {
        IGroupByTyped<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> builder);
    }
}