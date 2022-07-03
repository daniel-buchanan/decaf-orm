using System;
using System.Linq.Expressions;
using pdq.common;

namespace pdq
{
    public interface IOrderByTyped<T> : IExecute
    {
        IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2> : IExecute
    {
        IOrderByThenTyped<T1, T2> OrderBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3, T4> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3, T4, T5> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T> : IExecute
    {
        IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2> : IExecute
    {
        IOrderByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3, T4> ThenBy(Expression<Func<T1, T2, T3, T4, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4, T5> : IExecute
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending);
    }
}

