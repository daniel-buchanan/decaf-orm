using decaf.common.Connections;
using decaf.common.Options;
using Microsoft.Extensions.DependencyInjection;
using decaf.db.common;

namespace decaf.tests.common.Mocks
{
	public static class DecafExtensions
	{
		public static IDecafOptionsBuilder UseMockDatabase(this IDecafOptionsBuilder builder, bool throwOnCommit = false)
		{
			var options = new MockDatabaseOptions();
			if(throwOnCommit)
				options.ThrowExceptionOnCommit();
			
			var x = builder as IDecafOptionsBuilderExtensions;
			x.Services.AddSingleton(options);
			x.UseDbImplementation<MockDbImplementationFactory>(options);
			return builder;
		}

		public static IDecafOptionsBuilder WithMockConnectionDetails(this IDecafOptionsBuilder builder)
		{
			var x = builder as IDecafOptionsBuilderExtensions;
			x.Services.AddScoped<IConnectionDetails, MockConnectionDetails>();
			return builder;
		}
	}
}

