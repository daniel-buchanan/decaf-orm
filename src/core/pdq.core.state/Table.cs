using System;
namespace pdq.state
{
	public class Table
	{
		private Table()
        {
			Id = Guid.NewGuid();
        }

		private Table(string name, string alias, string schema) : this()
		{
			Name = name;
			Alias = alias;
			Schema = schema;
		}

		public Guid Id { get; private set; }

		public string Name { get; private set; }

		public string Alias { get; private set; }

		public string Schema { get; private set; }

		public static Table Create(string name, string alias = null, string schema = null) => new Table(name, alias, schema);
	}
}

