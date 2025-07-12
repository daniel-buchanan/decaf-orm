using System;
using System.Collections.Generic;

namespace decaf.common.Utilities.Reflection.Dynamic;

public class DynamicColumnInfo : IComparable<DynamicColumnInfo>
{
	private DynamicColumnInfo(
		string? name,
		string? newName,
		object? value,
		Type? valueType,
		Type? type,
		string? alias,
		IValueFunction? function)
	{
		this.name = name;
		this.newName = newName;
		this.value = value;
		this.valueType = valueType;
		this.type = type;
		this.alias = alias;
		this.function = function;
	}

	public static DynamicColumnInfo Empty()
		=> new(null, null, null, null, null, null, null);

	public static DynamicColumnInfo Create(
		string? name = null,
		string? newName = null,
		object? value = null,
		Type? valueType = null,
		Type? type = null,
		string? alias = null,
		IValueFunction? function = null)
		=> new(name, newName, value, valueType, type, alias, function);

	private string? name;
	private string? newName;
	private object? value;
	private Type? valueType;
	private Type? type;
	private string? alias;
	private IValueFunction? function;
	
	public string? Name => name;

	public string? NewName => newName;

	public object? Value => value;

	public Type? ValueType => valueType;

	public Type? Type => type;

	public string? Alias => alias;

	public IValueFunction? Function => function;

	public void SetName(string val) => name = val;

	public void SetNewName(string val) => newName = val;

	public void SetValue(object? val) => value = val;

	public void SetValueType(Type? val) => valueType = val;

	public void SetType(Type val) => type = val;

	public void SetAlias(string val) => alias = val;

	public void SetFunction(IValueFunction val) => function = val;

	private bool IsEquivalentTo(DynamicColumnInfo? columnInfo)
	{
		if (columnInfo is null) return false;

		return Name == columnInfo.Name &&
		       NewName == columnInfo.NewName &&
		       Alias == columnInfo.Alias &&
		       Type == columnInfo.Type;
	}

	public int CompareTo(DynamicColumnInfo other)
		=> IsEquivalentTo(other) ? 1 : 0;

	public override bool Equals(object? obj)
	{
		return obj is DynamicColumnInfo info &&
		       Name == info.Name &&
		       NewName == info.NewName &&
		       Alias == info.Alias;
	}

	public override int GetHashCode()
	{
		int hashCode = 638763575;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name ?? string.Empty);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NewName ?? string.Empty);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Alias ?? string.Empty);
		return hashCode;
	}

	private static int Compare(DynamicColumnInfo left, DynamicColumnInfo right)
		=> left.CompareTo(right);

	public static bool operator ==(DynamicColumnInfo left, DynamicColumnInfo? right)
	{
		if (ReferenceEquals(left, null))
		{
			return ReferenceEquals(right, null);
		}
		return left.Equals(right);
	}

	public static bool operator !=(DynamicColumnInfo left, DynamicColumnInfo right)
		=> !(left == right);

	public static bool operator >(DynamicColumnInfo left, DynamicColumnInfo right)
		=> Compare(left, right) > 0;
        
	public static bool operator >=(DynamicColumnInfo left, DynamicColumnInfo right)
		=> Compare(left, right) > 0;

	public static bool operator <(DynamicColumnInfo left, DynamicColumnInfo right)
		=> Compare(left, right) < 0;
        
	public static bool operator <=(DynamicColumnInfo left, DynamicColumnInfo right)
		=> Compare(left, right) < 0;
}