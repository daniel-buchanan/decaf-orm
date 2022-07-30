using System;

namespace pdq
{
	public interface IJoin
	{
        /// <summary>
        /// Join to another table.
        /// </summary>
        /// <returns>(FluentApi) The ability to specify the table you are joining from.</returns>
        IJoinFrom Join();

        /// <summary>
        /// Perform an Inner join to another table.
        /// </summary>
        /// <returns>(FluentApi) The ability to specify the table you are joining from.</returns>
        IJoinFrom InnerJoin();

        /// <summary>
        /// Perform a Left join to another table.
        /// </summary>
        /// <returns>(FluentApi) The ability to specify the table you are joining from.</returns>
        IJoinFrom LeftJoin();

        /// <summary>
        /// Perform a Right join to another table.
        /// </summary>
        /// <returns>(FluentApi) The ability to specify the table you are joining from.</returns>
        IJoinFrom RightJoin();
	}

    public interface IJoinFrom
    {
        /// <summary>
        /// Specify the table that forms the "left" portion of the join.
        /// </summary>
        /// <param name="name">The name of the table to join from.</param>
        /// <param name="alias">The alias of the table.</param>
        /// <param name="schema">(Optional) The schema the table belongs to.</param>
        /// <returns>(FluentApi) The ability to specify the table you are joining to.</returns>
        IJoinTo From(string name, string alias, string schema = null);
    }

    public interface IJoinTo
    {
        /// <summary>
        /// Specify the table that forms the "right" portion of the join.
        /// </summary>
        /// <param name="name">The name of the table to join to.</param>
        /// <param name="alias">The alias of the table.</param>
        /// /// <param name="schema">(Optional) The schema the table belongs to.</param>
        /// <returns>(FluentApi) The ability to specify the conditions on which to join.</returns>
        IJoinConditions To(string name, string alias, string schema = null);

        /// <summary>
        /// Specify a sub-query to join to, and give it an alias as the "right" portion
        /// of the join.
        /// </summary>
        /// <param name="query">The query that you wish to join to.</param>
        /// <returns>(FluentApi) The ability to specify the conditions on which to join.</returns>
        IJoinConditions To(Action<ISelectWithAlias> query);
    }

    public interface IJoinConditions
    {
        /// <summary>
        /// Specify the conditions on which the tables should be joined.
        /// </summary>
        /// <param name="builder">A builder to specify the conditions.</param>
        /// <returns>(FluentApi) The ability to join to more tables, select columns etc.</returns>
        ISelectFrom On(Action<IWhereBuilder> builder);
    }
}
