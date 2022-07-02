using System;
using System.Linq.Expressions;

namespace pdq
{
	public interface ISelectColumnTyped<T>
	{
        ISelectFromTyped<T> Columns(Expression<Func<T, dynamic>> selectExpression);

        ISelectFromTyped<T> Column(Expression<Func<T, object>> selectExpression);
	}

    public interface ISelectColumnTyped<T1, T2>
    {
        ISelectFromTyped<T1, T2> Columns(Expression<Func<T2, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2> Columns(Expression<Func<T1, T2, dynamic>> selectExpression);

        ISelectFromTyped<T1, T2> Column(Expression<Func<T2, object>> selectExpression);
    }

    public interface ISelectColumnTyped<T1, T2, T3>
    {
        ISelectFromTyped<T1, T2, T3> Columns(Expression<Func<T3, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3> Columns(Expression<Func<T1, T2, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3> Columns(Expression<Func<T1, T2, T3, dynamic>> selectExpression);

        ISelectFromTyped<T1, T2, T3> Column(Expression<Func<T3, object>> selectExpression);
    }

    public interface ISelectColumnTyped<T1, T2, T3, T4>
    {
        ISelectFromTyped<T1, T2, T3, T4> Columns(Expression<Func<T4, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4> Columns(Expression<Func<T1, T2, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4> Columns(Expression<Func<T1, T2, T3, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4> Columns(Expression<Func<T1, T2, T3, T4, dynamic>> selectExpression);

        ISelectFromTyped<T1, T2, T3, T4> Column(Expression<Func<T4, object>> selectExpression);
    }

    public interface ISelectColumnTyped<T1, T2, T3, T4, T5>
    {
        ISelectFromTyped<T1, T2, T3, T4, T5> Columns(Expression<Func<T5, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4, T5> Columns(Expression<Func<T1, T2, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4, T5> Columns(Expression<Func<T1, T2, T3, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4, T5> Columns(Expression<Func<T1, T2, T3, T4, dynamic>> selectExpression);
        ISelectFromTyped<T1, T2, T3, T4, T5> Columns(Expression<Func<T1, T2, T3, T4, T5, dynamic>> selectExpression);

        ISelectFromTyped<T1, T2, T3, T4, T5> Column(Expression<Func<T5, object>> selectExpression);
    }
}

