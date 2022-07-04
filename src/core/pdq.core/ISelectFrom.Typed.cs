using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface ISelectFromTyped<T> :
		IJoinTyped<T>,
		IDisposable
	{
		IGroupByTyped<T> Where(Expression<Func<T, bool>> builder);
	}

    public interface ISelectFromTyped<T1, T2> :
        IJoinTyped<T1, T2>,
        IDisposable
    {
        IGroupByTyped<T1, T2> Where(Expression<Func<T1, T2, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3> :
        IJoinTyped<T1, T2, T3>,
        IDisposable
    {
        IGroupByTyped<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4> :
        IJoinTyped<T1, T2, T3, T4>,
        IDisposable
    {
        IGroupByTyped<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> builder);
    }

    public interface ISelectFromTyped<T1, T2, T3, T4, T5> :
        IDisposable
    {
        IGroupByTyped<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> builder);
    }
}