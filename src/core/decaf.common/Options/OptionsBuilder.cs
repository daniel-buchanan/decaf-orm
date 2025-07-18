﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using decaf.common.Utilities;
using decaf.common.Utilities.Reflection;

namespace decaf.common.Options;

public class OptionsBuilder<T> : IOptionsBuilder<T>
    where T: class, new()
{
    private readonly Dictionary<string, object?> values = new Dictionary<string, object?>();
    private readonly ExpressionHelper expressionHelper;

    public OptionsBuilder()
    {
        var reflectionHelper = new ReflectionHelper();
        expressionHelper = new ExpressionHelper(reflectionHelper);
    }

    /// <inheritdoc/>
    public T Build()
    {
        var options = new T();
        var optionsType = typeof(T);
        var flags = BindingFlags.Instance | BindingFlags.Public;
        var properties = optionsType.GetProperties(flags);

        foreach(var p in properties)
        {
            if (!values.TryGetValue(p.Name, out var value))
                continue;

            p.SetValue(options, value);
        }

        return options;
    }

    /// <inheritdoc />
    public void ConfigureProperty<TValue>(Expression<Func<T, TValue>> expr, TValue value)
    {
        var prop = expressionHelper.GetMemberName(expr);
        if (prop.IsNullOrWhiteSpace()) return;
        ConfigureProperty(prop!, value);
    }

    /// <summary>
    /// Configure a property to be set when the options are built.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to add.</typeparam>
    /// <param name="property">The property to configure.</param>
    /// <param name="value">The value to set the property to.</param>
    protected void ConfigureProperty<TValue>(string property, TValue value)
        => values.Add(property, value);
}