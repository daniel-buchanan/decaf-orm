using System;
using Moq;
using pdq.common.Logging;

namespace pdq.tests.common.Mocks
{
    public class MockLoggerProxyProxy : DefaultLoggerProxy
    {
        public MockLoggerProxyProxy(PdqOptions options) : 
            base(options, Mock.Of<IStdOutputWrapper>()) { }
    }
}

