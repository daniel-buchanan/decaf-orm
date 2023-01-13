using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface ISelectFromTyped<T> :
        IExecute,
        ISelectColumnTyped<T>,
		IJoinTyped<T>
	{
        ISelectFromTyped<T, T1> From<T1>();
        ISelectFromTyped<T, T1> From<T1>(Expression<Func<T1, T1>> expression);
		IGroupByTyped<T> Where(Expression<Func<T, bool>> builder);
	}

    public interface ISelectFromTyped<T1, T2> :
        IExecute,
        IJoinTyped<T1, T2>
    {
        ISelectFromTyped<T1, T2, T3> From<T3>();
        ISelectFromTyped<T1, T2, T3> From<T3>(Expression<Func<T3, T3>> expression);
        IGroupByTyped<T1, T2> Where(Expression<Func<T1, T2, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3> :
        IExecute,
        IJoinTyped<T1, T2, T3>
    {
        ISelectFromTyped<T1, T2, T3, T4> From<T4>();
        ISelectFromTyped<T1, T2, T3, T4> From<T4>(Expression<Func<T4, T4>> expression);
        IGroupByTyped<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4> :
        IExecute,
        IJoinTyped<T1, T2, T3, T4>
    {
        ISelectFromTyped<T1, T2, T3, T4, T5> From<T5>();
        ISelectFromTyped<T1, T2, T3, T4, T5> From<T5>(Expression<Func<T5, T5>> expression);
        IGroupByTyped<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4, T5> : IExecute
    {
        IGroupByTyped<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> builder);
    }
}