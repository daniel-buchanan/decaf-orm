using System;
using System.Linq.Expressions;
using pdq.common;

namespace pdq
{
    public interface IOrderByTyped<T> :
        IExecute,
        ISelectColumnTyped<T>
    {
        /// <summary>
        /// Order by a given property
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IOrderByThenTyped<T> OrderBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2> :
        IExecute,
        ISelectColumnTyped<T1, T2>
    {
        IOrderByThenTyped<T1, T2> OrderBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3>
    {
        IOrderByThenTyped<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3, T4> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3, T4>
    {
        IOrderByThenTyped<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByTyped<T1, T2, T3, T4, T5> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3, T4, T5>
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T> :
        IExecute,
        ISelectColumnTyped<T>
    {
        IOrderByThenTyped<T> ThenBy(Expression<Func<T, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2> :
        IExecute,
        ISelectColumnTyped<T1, T2>
    {
        IOrderByThenTyped<T1, T2> ThenBy(Expression<Func<T1, T2, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3>
    {
        IOrderByThenTyped<T1, T2, T3> ThenBy(Expression<Func<T1, T2, T3, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3, T4>
    {
        IOrderByThenTyped<T1, T2, T3, T4> ThenBy(Expression<Func<T1, T2, T3, T4, object>> builder, SortOrder order = SortOrder.Ascending);
    }

    public interface IOrderByThenTyped<T1, T2, T3, T4, T5> :
        IExecute,
        ISelectColumnTyped<T1, T2, T3, T4, T5>
    {
        IOrderByThenTyped<T1, T2, T3, T4, T5> ThenBy(Expression<Func<T1, T2, T3, T4, T5, object>> builder, SortOrder order = SortOrder.Ascending);
    }
}

