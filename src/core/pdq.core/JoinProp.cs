using System;
namespace pdq.core
{
	public class JoinProp
	{
		private JoinProp(string table, string alias, string column)
        {
			Table = table;
			TableAlias = alias;
			Column = column;
        }

		public string Table { get; }
		public string Column { get; }
		public string TableAlias { get; }

		public static JoinProp Create(string table, string alias, string column)
        {
			return new JoinProp(table, alias, column);
        }
	}
}

