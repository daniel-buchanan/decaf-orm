using Microsoft.Extensions.DependencyInjection;
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
			x.UseDbImplementation<MockDbImplementationFactory>(new MockDatabaseOptions());
			return builder;
		}

		public static IPdqOptionsBuilder WithMockConnectionDetails(this IPdqOptionsBuilder builder)
		{
			var x = builder as IPdqOptionsBuilderExtensions;
			x.Services.AddScoped<IConnectionDetails, MockConnectionDetails>();
			return builder;
		}
	}
}

