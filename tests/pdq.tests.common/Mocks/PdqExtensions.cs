using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Options;
using pdq.tests.common.Mocks;

namespace pdq.tests.common.Mocks
{
	public static class PdqExtensions
	{
		public static IPdqOptionsBuilder UseMockDatabase(this IPdqOptionsBuilder builder)
        {
			builder.ConfigureDbImplementation<MockSqlFactory, MockConnectionFactory, MockTransactionFactory>();
			builder.Services.AddScoped<IConnectionDetails, MockConnectionDetails>();
			return builder;
        }
	}
}

