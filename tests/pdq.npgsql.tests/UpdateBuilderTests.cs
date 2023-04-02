using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using Xunit;

namespace pdq.npgsql.tests
{
	public class UpdateBuilderTests
	{
		private readonly IQueryContainer query;

		public UpdateBuilderTests()
		{
			var services = new ServiceCollection();
			services.AddPdq(o =>
			{
				o.EnableTransientTracking();
				o.OverrideDefaultLogLevel(LogLevel.Debug);
				o.DisableSqlHeaderComments();
				o.UseNpgsql(options =>
				{

				});
			});
			services.AddScoped<IConnectionDetails>(s => new NpgsqlConnectionDetails());

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            this.query = transient.Query() as IQueryContainer;
        }

		[Fact]
		public void SimpleUpdateSucceeds()
		{
			// Arrange
			var expected = "update\\r\\n  users\\r\\nset\\r\\n  sub = @p1\\r\\nwhere\\r\\n(\\r\\n  (id = @p2)\\r\\n)\\r\\n";
			expected = expected.Replace("\\r\\n", Environment.NewLine);
			var idValue = Guid.NewGuid();

            // Act
            var q = this.query.Update()
				.Table("users")
                .Set(new {
					sub = "abc123"
				})
                .Where(b => b.Column("id").Is().EqualTo(idValue));

			var sql = q.GetSql();

			// Assert
			sql.Should().Be(expected);
		}
    }
}

