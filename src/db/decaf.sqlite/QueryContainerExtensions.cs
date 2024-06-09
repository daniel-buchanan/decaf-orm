using decaf.common;
using decaf.state;

namespace decaf.sqlite;

public static class QueryContainerExtensions
{
    /// <summary>
    /// Begin a create query, this will allow you to crete a table.
    /// </summary>
    /// <param name="query">The query instance that will become a "create table" query.</param>
    /// <returns>(FluentApi) The ability to specify the table to create.</returns>
    /// <example>q.CreateTable().Named("users").WithColumns(...).WithPrimaryKey(...);</example>
    public static ICreateTable CreateTable(this IQueryContainer query)
        => Implementation.CreateTable.Create(query.CreateContext<ICreateTableQueryContext>(), query);
}