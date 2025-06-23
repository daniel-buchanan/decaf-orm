using System;
using decaf.common.Connections;
using decaf.common.Options;
using Microsoft.Extensions.DependencyInjection;
using decaf.db.common;

namespace decaf.tests.common.Mocks;

public static class DecafExtensions
{
	public static IDecafOptionsBuilder UseMockDatabase(this IDecafOptionsBuilder builder)
	{
		var options = new MockDatabaseOptions();
		var x = builder as IDecafOptionsBuilderExtensions;
		x.UseDbImplementation<MockDbImplementationFactory>(options);
		return builder;
	}
		
	public static IDecafOptionsBuilder UseMockDatabase(this IDecafOptionsBuilder builder, MockDatabaseOptions options)
	{
		options = options ?? new MockDatabaseOptions();
			
		var x = builder as IDecafOptionsBuilderExtensions;
		x.UseDbImplementation<MockDbImplementationFactory>(options);
		return builder;
	}

	public static IDecafOptionsBuilder UseMockDatabase(this IDecafOptionsBuilder builder,
		Action<IMockDbOptionsBuilder> optionsBuilder)
	{
		var mob = new MockDbOptionsBuilder();
		optionsBuilder.Invoke(mob);

		var options = mob.Build();
		var x = builder as IDecafOptionsBuilderExtensions;
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