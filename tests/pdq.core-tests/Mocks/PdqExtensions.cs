using System;
using pdq.common;
using pdq.core_tests.Mocks;

namespace pdq.core_tests.Mocks
{
	public static class PdqExtensions
	{
		public static void UseMockDatabase(this IPdqOptionsBuilder optionsBuilder)
        {
			var builder = optionsBuilder as IPdqOptionsBuilderInternal;
			builder.SetConnectionFactory<MockConnectionFactory>();
			builder.SetTransactionFactory<MockTransactionFactory>();
			builder.SetSqlFactory<MockSqlFactory>();
			builder.SetLoggerProxy<MockLoggerProxy>();
        }
	}
}

