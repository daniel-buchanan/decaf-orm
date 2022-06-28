namespace pdq
{
	public interface IDelete : IBuilder
	{
        /// <summary>
        /// Sets the initial table, view or other schema item to perform the
        /// delete query on.
        /// </summary>
        /// <param name="name">The name of the table or view etc.</param>
        /// <param name="alias">(optional) The alias to give this source in the query.</param>
        /// <param name="schema">(optional) The schema to which this source belongs to.</param>
        /// <returns>(FluentApi) The ability to restrict the scope of the query an the execute it.</returns>
        /// <example>.From("users", "u")</example>
        /// <example>.From("users", "u", "transactional")</example>
        IDeleteFrom From(string name, string alias = null, string schema = null);
	}
}

