using System;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using Xunit;

namespace pdq.npgsql.tests
{
	public class SelectBuilderTests
	{
		private readonly IQuery query;

		public SelectBuilderTests()
		{
			var services = new ServiceCollection();
			services.AddPdq(o =>
			{
				o.EnableTransientTracking();
				o.OverrideDefaultLogLevel(LogLevel.Debug);
				o.UseNpgsql(options =>
				{

				});
			});
			services.AddScoped<IConnectionDetails>(s => new NpgsqlConnectionDetails());

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            this.query = transient.Query() as IQuery;
        }

		[Fact]
		public void SimpleSelectSucceeds()
		{
			// Arrange

			// Act
			var q = this.query.Select()
				.From("users", "u")
				.Where(b =>
				{
					b.Column("sub").Is().EqualTo(Guid.NewGuid());
				})
				.Select(c => new
				{
					email = c.Is("email"),
					id = c.Is("sub")
				});
			var sql = q.GetSql();
			var x = sql;

			// Assert
		}
	}
}

