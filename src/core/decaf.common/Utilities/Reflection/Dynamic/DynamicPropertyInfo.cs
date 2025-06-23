using System;
using System.Globalization;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic;

public class DynamicPropertyInfo : PropertyInfo
{
    private readonly string name;
    private readonly Type propertyType;
    private readonly Type declaringType;

    public DynamicPropertyInfo(string name, Type propertyType, Type declaringType)
    {
        this.name = name;
        this.propertyType = propertyType;
        this.declaringType = declaringType;
    }

    public override PropertyAttributes Attributes => PropertyAttributes.None;

    public override bool CanRead => true;

    public override bool CanWrite => true;

    public override Type PropertyType => propertyType;

    public override Type DeclaringType => declaringType;

    public override string Name => name;

    public override Type ReflectedType => PropertyType;

    public override MethodInfo[] GetAccessors(bool nonPublic) => new MethodInfo[] { };

    public override object[] GetCustomAttributes(bool inherit) => new object[] { };

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[] { };

    public override MethodInfo GetGetMethod(bool nonPublic) => new DynamicMethodInfo($"get_{name}", propertyType);

    public override ParameterInfo[] GetIndexParameters() => new ParameterInfo[] { };

    public override MethodInfo GetSetMethod(bool nonPublic)
        => new DynamicMethodInfo($"set_{name}", propertyType, new DynamicParameterInfo("value", propertyType));

    public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}