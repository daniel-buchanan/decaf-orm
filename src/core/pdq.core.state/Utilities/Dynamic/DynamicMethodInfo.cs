using System;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

namespace pdq.state.Utilities
{
    public class DynamicMethodInfo : MethodInfo
    {
        private readonly string name;
        private readonly Type reflectedType;
        private readonly ParameterInfo[] parameters;

        public DynamicMethodInfo(string name, Type reflectedType, params ParameterInfo[] parameters)
        {
            this.name = name;
            this.reflectedType = reflectedType;
            this.parameters = parameters;
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes => null;

        public override MethodAttributes Attributes => MethodAttributes.Public;

        public override RuntimeMethodHandle MethodHandle => new RuntimeMethodHandle();

        public override Type DeclaringType => typeof(DynamicObject);

        public override string Name => this.name;

        public override Type ReflectedType => this.reflectedType;

        public override MethodInfo GetBaseDefinition() => this;

        public override object[] GetCustomAttributes(bool inherit) => new object[] { };

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[] { };

        public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.Managed;

        public override ParameterInfo[] GetParameters() => this.parameters;

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
            => base.Invoke(obj, parameters);

        public override bool IsDefined(Type attributeType, bool inherit) => true;
    }
}

