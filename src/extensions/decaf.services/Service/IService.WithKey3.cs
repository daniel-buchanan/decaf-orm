﻿using System;
namespace decaf.services;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey1"></typeparam>
/// <typeparam name="TKey2"></typeparam>
/// <typeparam name="TKey3"></typeparam>
public interface IService<TEntity, TKey1, TKey2, TKey3> :
  IQuery<TEntity, TKey1, TKey2, TKey3>,
  ICommand<TEntity, TKey1, TKey2, TKey3>
  where TEntity : IEntity<TKey1, TKey2, TKey3>, new()
{
  /// <summary>
  /// Event fired before the query is executed.
  /// </summary>
  new event EventHandler<PreExecutionEventArgs> OnBeforeExecution;
}