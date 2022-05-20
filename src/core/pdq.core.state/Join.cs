using System;
namespace pdq.core.state
{
	public class Join
	{
		private Join(Table table, IWhere conditions)
		{
			Table = table;
			Conditions = conditions;
		}

		public Table Table { get; private set; }

		public IWhere Conditions { get; private set; }

		public static Join Create(Table table, IWhere conditions)
        {
			return new Join(table, conditions);
        }
	}
}

