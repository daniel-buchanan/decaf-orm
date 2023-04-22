using System;
using pdq.common.Logging;

namespace pdq.tests.common.Mocks
{
    public class MockLoggerProxy : DefaultLogger
    {
        public MockLoggerProxy(PdqOptions options) : base(options)
        {
        }
    }
}

