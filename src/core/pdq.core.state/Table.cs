using System;
namespace pdq.core.state
{
	public class Table
	{
		private Table()
        {
			Id = Guid.NewGuid();
        }

		private Table(string alias) : this()
        {
			Alias = alias;
        }

		private Table(string name, string alias) : this(alias)
		{
			Name = name;
		}

		public Guid Id { get; private set; }

		public string? Name { get; private set; }

		public string? Alias { get; private set; }

		public static Table Create(string alias) => new Table(alias);

        public static Table Create(string name, string alias) => new Table(name, alias);
    }
}

