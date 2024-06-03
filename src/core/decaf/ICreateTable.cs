using System;
using System.Linq.Expressions;

namespace decaf;

public interface ICreateTable : IExecute
{
    /// <summary>
    /// Specify the name for the table to be created.
    /// </summary>
    /// <param name="name">The name to use for the table.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable Named(string name);

    /// <summary>
    /// Create a table with the columns as provided by the array of statements.
    /// </summary>
    /// <param name="columns">The set of columns to create.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable WithColumns(params Expression<Action<IDdlColumnBuilder>>[] columns);

    /// <summary>
    /// Create an index using the provided name and columns.
    /// </summary>
    /// <param name="name">The name of the index to create.</param>
    /// <param name="table">The table this index is on.</param>
    /// <param name="columns">The columns to be included in the index.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable WithIndex(string name, string table, params Expression<Action<IDdlColumnBuilder>>[] columns);
    
    /// <summary>
    /// Create an index using the provided columns.
    /// </summary>
    /// <param name="table">The table this index is on.</param>
    /// <param name="columns">The columns to be included in the index.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable WithIndex(string table, params Expression<Action<IDdlColumnBuilder>>[] columns);

    /// <summary>
    /// Add a primary key to the table being created. 
    /// </summary>
    /// <param name="columns"></param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable WithPrimaryKey(params Expression<Action<IDdlColumnBuilder>>[] columns);

    /// <summary>
    /// Add a primary key to the table being created.
    /// </summary>
    /// <param name="name">The name of the primary key to be created.</param>
    /// <param name="columns">The columns to include in the primary key.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable WithPrimaryKey(string name, params Expression<Action<IDdlColumnBuilder>>[] columns);
}

public interface ICreateTable<T> : IExecute<T>
{
    /// <summary>
    /// Use the name of the provided Type as the table name.
    /// </summary>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> UsingTypeName();
    
    /// <summary>
    /// Specify the name for the table to be created.
    /// </summary>
    /// <param name="name">The name to use for the table.</param>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> Named(string name);
    
    /// <summary>
    /// Create an index using the provided name and columns.
    /// </summary>
    /// <param name="name">The name of the index to create.</param>
    /// <param name="columns">The columns to be included in the index.</param>
    /// <typeparam name="T">The type to get columns from.</typeparam>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> WithIndex(string name, params Expression<Func<T, object>>[] columns);
    
    /// <summary>
    /// Create an index using the provided columns.
    /// </summary>
    /// <param name="columns">The columns to be included in the index.</param>
    /// <typeparam name="T">The type to get columns from.</typeparam>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> WithIndex(params Expression<Func<T, object>>[] columns);
    
    /// <summary>
    /// Add a primary key to the table being created.
    /// </summary>
    /// <param name="columns">The columns to include in the primary key.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> WithPrimaryKey(params Expression<Func<T, object>>[] columns);

    /// <summary>
    /// Add a primary key to the table being created.
    /// </summary>
    /// <param name="name">The name of the primary key to be created.</param>
    /// <param name="columns">The columns to include in the primary key.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>(FluentAPI) The ability to continue to create a table.</returns>
    ICreateTable<T> WithPrimaryKey(string name, params Expression<Func<T, object>>[] columns);
}