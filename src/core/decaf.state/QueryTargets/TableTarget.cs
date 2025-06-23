using System;
using decaf.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.core.tests")]
namespace decaf.state.QueryTargets;

public class TableTarget : ITableTarget
{
	private TableTarget()
	{
		Id = Guid.NewGuid();
	}

	private TableTarget(string name, string alias, string schema) : this()
	{
		Name = name;
		Alias = alias;
		Schema = schema;
	}

	public Guid Id { get; private set; }

	public string Name { get; private set; }

	public string Alias { get; private set; }

	public string Schema { get; private set; }

	public static TableTarget Create(string name, string alias = null, string schema = null) => new TableTarget(name, alias, schema);

	public bool IsEquivalentTo(ITableTarget target)
	{
		var minimum = target.Name == Name &&
		              target.Alias == Alias;

		if (!minimum) return false;

		var sameSchema = target.Schema == Schema;
		return sameSchema;
	}

	public bool IsEquivalentTo(IQueryTarget target)
	{
		return target.Alias == Alias;
	}
}