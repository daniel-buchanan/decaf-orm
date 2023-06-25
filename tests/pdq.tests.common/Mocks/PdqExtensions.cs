using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Options;
using pdq.db.common;

namespace pdq.tests.common.Mocks
{
    public static class PdqExtensions
	{
		public static IPdqOptionsBuilder UseMockDatabase(this IPdqOptionsBuilder builder)
        {
			var x = builder as IPdqOptionsBuilderExtensions;
			x.Services.AddSingleton<ISqlFactory, MockSqlFactory>();
			x.Services.AddSingleton<IConnectionFactory, MockConnectionFactory>();
			x.Services.AddSingleton<ITransactionFactory, MockTransactionFactory>();
			x.Services.AddScoped<IConnectionDetails, MockConnectionDetails>();
			return builder;
			
        }
    }
}

