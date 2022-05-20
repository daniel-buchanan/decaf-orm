using System;
namespace pdq.core.state
{
	public class Table
	{
		private Table(string name, string alias)
		{
			Id = Guid.NewGuid();
			Name = name;
			Alias = alias;
		}

		public Guid Id { get; private set; }

		public string Name { get; private set; }

		public string Alias { get; private set; }

		public static Table Create(string name, string alias)
        {
			return new Table(name, alias);
        }
	}
}

