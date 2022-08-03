using System;
using pdq.common.Logging;

namespace pdq.core_tests.Mocks
{
    public class MockLoggerProxy : DefaultLogger
    {
        public MockLoggerProxy(PdqOptions options) : base(options)
        {
        }
    }
}

