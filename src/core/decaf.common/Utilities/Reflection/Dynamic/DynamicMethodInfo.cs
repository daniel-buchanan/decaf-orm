using System;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic;

public class DynamicMethodInfo(string name, Type reflectedType, params ParameterInfo[] inputParameters)
    : MethodInfo
{
    public override ICustomAttributeProvider ReturnTypeCustomAttributes => new MockCustomAttributeProvider();

    public override MethodAttributes Attributes => MethodAttributes.Public;

    public override RuntimeMethodHandle MethodHandle => new RuntimeMethodHandle();

    public override Type DeclaringType => typeof(DynamicObject);

    public override string Name => name;

    public override Type ReflectedType => reflectedType;

    public override MethodInfo GetBaseDefinition() => this;

    public override object[] GetCustomAttributes(bool inherit) => [];

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => [];

    public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.Managed;

    public override ParameterInfo[] GetParameters() => inputParameters;

    public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        => base.Invoke(obj, parameters);

    public override bool IsDefined(Type attributeType, bool inherit) => true;
}