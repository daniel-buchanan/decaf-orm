using System;
using System.Reflection;

namespace decaf.common.Utilities.Reflection.Dynamic
{
    public class DynamicParameterInfo : ParameterInfo
    {
        private readonly string name;
        private readonly Type parameterType;

        public DynamicParameterInfo(string name, Type parameterType)
        {
            this.name = name;
            this.parameterType = parameterType;
        }

        public override string Name => name;

        public override Type ParameterType => parameterType;
    }
}

