using System;
using System.Linq.Expressions;

namespace pdq
{
    public interface IOrderByTyped<T> : IExecute
    {
        IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder);
    }

    public interface IOrderByTyped<T1, T2> : IExecute, IOrderByTyped<T1>
    {
        IOrderByThenTyped<T1, T2> OrderBy(Expression<Func<T1, T2, object>> builder);
    }

    public interface IOrderByTyped<T1, T2, T3> : IExecute, IOrderByTyped<T1, T2>
    {
        IOrderByThenTyped<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> builder);
    }

    public interface IOrderByTyped<T1, T2, T3, T4> : IExecute, IOrderByTyped<T1, T2, T3>
    {
        IOrderByThenTyped<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> builder);
    }

    public interface IOrderByTyped<T1, T2, T3, T4, T5> : IExecute, IOrderByTyped<T1, T2, T3, T4>
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder);
    }

    public interface IOrderByThenTyped<T> : IExecute
    {
        IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder);
    }

    public interface IOrderByThenTyped<T1, T2> : IExecute, IOrderByThenTyped<T1>
    {
        IOrderByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder);
    }

    public interface IOrderByThenTyped<T1, T2, T3> : IExecute, IOrderByThenTyped<T1, T2>
    {
        IOrderByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4> : IExecute, IOrderByThenTyped<T1, T2, T3>
    {
        IOrderByThenTyped<T1, T2, T3, T4> ThenBy(Expression<Func<T1, T2, T3, T4, object>> builder);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4, T5> : IExecute, IOrderByThenTyped<T1, T2, T3, T4>
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder);
    }
}

