using System;
using System.Collections.Generic;
using pdq.common.Utilities.Reflection;

namespace pdq.tests.common.Mocks
{
    public class MockValueParser : db.common.ValueParser
    {
        public MockValueParser(IReflectionHelper reflectionHelper) :
            base(reflectionHelper)
        { }

        protected override List<Tuple<string, string>> Replacements => throw new NotImplementedException();

        public override bool ValueNeedsQuoting(Type type)
            => true;

        protected override bool BooleanFromString(string input)
        {
            if (bool.TryParse(input, out var result))
                return result;
            return false;
        }

        protected override string BooleanToString(bool input)
            => input ? "True" : "False";

        protected override byte[] BytesFromString(string input)
            => System.Text.Encoding.UTF8.GetBytes(input);

        protected override string BytesToString(byte[] input)
            => System.Text.Encoding.UTF8.GetString(input);
    }
}