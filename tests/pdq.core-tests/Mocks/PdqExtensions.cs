using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Options;
using pdq.core_tests.Mocks;

namespace pdq.core_tests.Mocks
{
	public static class PdqExtensions
	{
		public static void UseMockDatabase(this IPdqOptionsBuilder builder)
        {
			builder.ConfigureDbImplementation<MockSqlFactory, MockConnectionFactory, MockTransactionFactory>();
			builder.Services.AddScoped<IConnectionDetails, MockConnectionDetails>();
        }
	}
}

