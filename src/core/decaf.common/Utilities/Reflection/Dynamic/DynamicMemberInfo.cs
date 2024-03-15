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

        public override Type DeclaringType => typeof(DynamicObject);

        public override MemberTypes MemberType => this.memberType;

        public override string Name => this.name;

        public override Type ReflectedType => this.reflectedType;

        public override object[] GetCustomAttributes(bool inherit) => new object[] { };

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[] { };

        public override bool IsDefined(Type attributeType, bool inherit) => false;
    }
}

