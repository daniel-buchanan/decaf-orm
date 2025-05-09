using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using decaf.common.Utilities.Reflection;
using decaf.ddl.Attributes;
using decaf.state.Ddl.Definitions;

namespace decaf.ddl.Utilities;

public class AttributeHelper(IReflectionHelper reflectionHelper, IExpressionHelper expressionHelper)
{
    private IEnumerable<IColumnDefinition> GetComponents<T, TAttribute>() where TAttribute : Attribute
    {
        var attrs = reflectionHelper.GetAttributes<T, TAttribute>();

        var components = new List<IColumnDefinition>();
        foreach (var a in attrs)
        {
            var cb = new DdlColumnBuilder(expressionHelper);
            cb.Named(a.AppliedTo)
                .AsType(a.AppliedType);
            components.Add(cb.Build());
        }

        return components;
    }

    public IPrimaryKeyDefinition GetPrimaryKey<T>()
    {
        var components = GetComponents<T, PrimaryKeyComponentAttribute>();
        return PrimaryKeyDefinition.Create<T>(components.ToArray());
    }
    
    public IPrimaryKeyDefinition GetPrimaryKey<T>(string name)
    {
        var components = GetComponents<T, PrimaryKeyComponentAttribute>();
        return PrimaryKeyDefinition.Create(name, components.ToArray());
    }

    public IIndexDefinition[] GetIndexes<T>()
    {
        var tbl = reflectionHelper.GetTableName<T>();
        var indexes = new List<IIndexDefinition>();
        var idxDefAttrs = reflectionHelper.GetAttributes<T, IndexAttribute>().ToArray();
        var idxCompAttrs = reflectionHelper.GetAttributes<T, IndexComponentAttribute>().ToArray();
        
        foreach (var def in idxDefAttrs)
        {
            if (def.Attribute is not IndexAttribute idx) continue;
            var cols = GetColumns<T>(idx.Name, idxCompAttrs) ?? [];
            if (cols.Length == 0) continue;
            indexes.Add(IndexDefinition.Create(idx.Name, tbl, cols));
        }

        return indexes.ToArray();
    }

    private static IColumnDefinition[] GetColumns<TSource>(string idx, IEnumerable<AttributeInfo> components)
    {
        var cols = new List<IColumnDefinition>();
        var type = typeof(TSource);
        foreach (var cmp in components)
        {
            if (cmp.Attribute is not IndexComponentAttribute idxCmp) continue;
            if (!string.Equals(idxCmp.IndexName, idx, StringComparison.OrdinalIgnoreCase)) continue;
            var parameter = Expression.Parameter(type);
            var memberInfo = type.GetProperty(cmp.AppliedTo);
            if (memberInfo is null) throw new InvalidOperationException($"Member {cmp.AppliedTo}, not found on class {type.Name}");
            var body = Expression.MakeMemberAccess(parameter, memberInfo);
            var lambda = Expression.Lambda(body, parameter);
            cols.Add(ColumnDefinitionBuilder.Build(lambda));
        }

        return cols.ToArray();
    }
}