using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic;

public class DynamicConstructorInfo : ConstructorInfo
{
    private readonly List<ParameterInfo> paramters;
    private readonly Type declaringType;

    public DynamicConstructorInfo(IEnumerable<DynamicColumnInfo> columns, Type declaringType)
    {
        this.declaringType = declaringType;
        paramters = new List<ParameterInfo>();
        foreach(var c in columns)
        {
            paramters.Add(new DynamicParameterInfo(c.NewName, c.ValueType));
        }
    }

    public override MethodAttributes Attributes => MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName;

    public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

    public override Type DeclaringType => declaringType;

    public override string Name => "Default";

    public override Type ReflectedType => throw new NotImplementedException();

    public override object[] GetCustomAttributes(bool inherit) => new object[] { };

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[] { };

    public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.Managed;

    public override ParameterInfo[] GetParameters() => paramters.ToArray();

    public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }
}