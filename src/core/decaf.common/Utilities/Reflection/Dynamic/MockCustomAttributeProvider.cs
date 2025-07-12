using System;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic;

public class MockCustomAttributeProvider : ICustomAttributeProvider
{
    public object[] GetCustomAttributes(bool inherit) => [];

    public object[] GetCustomAttributes(Type attributeType, bool inherit) => [];

    public bool IsDefined(Type attributeType, bool inherit) => true;
}