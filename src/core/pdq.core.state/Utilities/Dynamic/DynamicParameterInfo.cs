using System;
using System.Reflection;

namespace pdq.state.Utilities
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

        public override string Name => this.name;

        public override Type ParameterType => this.parameterType;
    }
}

