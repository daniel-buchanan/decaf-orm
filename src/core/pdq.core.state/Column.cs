using System;
namespace pdq.state
{
	public class Column
	{
		public string Name { get; private set; }

		public IQueryTarget Source { get; private set; }

		public string NewName { get; private set; }

		protected Column(string name, IQueryTarget source)
		{
			Name = name;
			Source = source;
		}

		protected Column(string name, IQueryTarget source, string newName) : this(name, source)
        {
			NewName = newName;
        }

        public static Column Create(string name, IQueryTarget source) => new Column(name, source);

        public static Column Create(string name, IQueryTarget source, string newName) => new Column(name, source, newName);

        public static Column Create(string name, string table, string tableAlias) => new Column(name, QueryTargets.TableTarget.Create(table, tableAlias));

		public bool IsEquivalentTo(Column column)
        {
			var minimum = column.Name == Name &&
				column.Source.Alias == Source.Alias;

			if (!minimum) return false;

			var sameName = column.NewName == NewName;
			return minimum || !sameName;
        }
    }
}

