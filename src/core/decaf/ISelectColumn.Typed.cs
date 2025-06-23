﻿using System;
using System.Linq.Expressions;

namespace decaf;

public interface ISelectColumnTyped<T>
{
    IExecuteDynamic SelectDynamic(Expression<Func<T, dynamic>> expression);
    IExecute<TResult> Select<TResult>(Expression<Func<T, TResult>> expression);
    IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression);
}

public interface ISelectColumnTyped<T1, T2>
{
    IExecuteDynamic SelectDynamic(Expression<Func<T1, T2, dynamic>> expression);
    IExecute<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> expression);
    IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression);
}

public interface ISelectColumnTyped<T1, T2, T3>
{
    IExecuteDynamic SelectDynamic(Expression<Func<T1, T2, T3, dynamic>> expression);
    IExecute<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> expression);
    IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression);
}

public interface ISelectColumnTyped<T1, T2, T3, T4>
{
    IExecuteDynamic SelectDynamic(Expression<Func<T1, T2, T3, T4, dynamic>> expression);
    IExecute<TResult> Select<TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression);
    IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression);
}

public interface ISelectColumnTyped<T1, T2, T3, T4, T5>
{
    IExecuteDynamic SelectDynamic(Expression<Func<T1, T2, T3, T4, T5, dynamic>> expression);
    IExecute<TResult> Select<TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> expression);
    IExecute<TResult> SelectAll<TResult>(Expression<Func<TResult, object>> expression);
}