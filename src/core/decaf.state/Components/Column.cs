using decaf.common;

namespace decaf.state;

public class Column : ColumnBase<Column>
{
	public string? NewName { get; private set; }

	protected Column(string name, IQueryTarget source) : base(name, source) { }

	protected Column(string name, IQueryTarget source, string newName) : this(name, source)
	{
		NewName = newName;
	}

	public static Column Create(string name, IQueryTarget source) => new(name, source);

	public static Column Create(string name, IQueryTarget source, string newName) => new(name, source, newName);

	public static Column Create(string name, string table, string tableAlias) => new(name, QueryTargets.TableTarget.Create(table, tableAlias));

	public override bool IsEquivalentTo(Column column)
	{
		var minimum = column.Name == Name &&
		              column.Source?.Alias == Source?.Alias;

		if (!minimum) return false;

		var sameName = column.NewName == NewName;
		return sameName;
	}
}