using decaf.common.Logging;
using Moq;

namespace decaf.tests.common.Mocks
{
    public class MockLoggerProxyProxy : DefaultLoggerProxy
    {
        public MockLoggerProxyProxy(DecafOptions options) : 
            base(options, Mock.Of<IStdOutputWrapper>()) { }
    }
}

