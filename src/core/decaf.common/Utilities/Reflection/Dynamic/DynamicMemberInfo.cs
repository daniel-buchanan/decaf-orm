using System;
using System.Dynamic;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic
{
    public class DynamicMemberInfo : MemberInfo
    {
        private readonly string name;
        private readonly Type reflectedType;
        private readonly MemberTypes memberType;

        public DynamicMemberInfo(string name, Type reflectedType, MemberTypes memberType)
        {
            this.name = name;
            this.reflectedType = reflectedType;
            this.memberType = memberType;
        }

        private Type declaringType = typeof(DynamicObject);

        public override Type DeclaringType => declaringType;

        public void SetDeclaringType(Type type) => declaringType = type;

        public override MemberTypes MemberType => memberType;

        public override string Name => name;

        public override Type ReflectedType => reflectedType;

        public override object[] GetCustomAttributes(bool inherit) => new object[] { };

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[] { };

        public override bool IsDefined(Type attributeType, bool inherit) => false;
    }
}

