using System;
namespace pdq.core.state
{
	public class Column
	{
		public string Name { get; private set; }

		public Table Table { get; private set; }

		public string? NewName { get; private set; }

		protected Column(string name, Table table)
		{
			Name = name;
			Table = table;
		}

		protected Column(string name, Table table, string newName) : this(name, table)
        {
			NewName = newName;
        }

        public static Column Create(string name, Table table) => new Column(name, table);

        public static Column Create(string name, Table table, string newName) => new Column(name, table, newName);

        public static Column Create(string name, string table, string alias) => new Column(name, Table.Create(name, alias));
    }
}

