using System;
namespace pdq.core.state
{
	public class Column
	{
		public string Name { get; private set; }

		public Table Table { get; private set; }

		protected Column(string name, Table table)
		{
			Name = name;
			Table = table;
		}

		public static Column Create(string name, Table table)
        {
			return new Column(name, table);
        }

		public static Column Create(string name, string table, string alias)
        {
			return new Column(name, Table.Create(name, alias));
        }
	}
}

