using System;
using FluentAssertions;
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
				o.DisableSqlHeaderComments();
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
			var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  sub = '@p1'\\r\\n)\\r\\n";
			expected = expected.Replace("\\r\\n", Environment.NewLine);
			var subValue = Guid.NewGuid();

			// Act
			var q = this.query.Select()
				.From("users", "u")
				.Where(b =>
				{
					b.Column("sub").Is().EqualTo(subValue);
				})
				.Select(c => new
				{
					email = c.Is("email"),
					id = c.Is("sub")
				});

			var sql = q.GetSql();

			// Assert
			sql.Should().Be(expected);
		}

        [Fact]
        public void SimpleSelectReturnsCorrectParameters()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  sub = '@p1'\\r\\n)";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.Column("sub").Is().EqualTo(subValue);
                })
                .Select(c => new
                {
                    email = c.Is("email"),
                    id = c.Is("sub")
                });

			var parameters = q.GetSqlParameters();

			// Assert
			parameters.Should().Satisfy(
				p => p.Key == "@p1" && ((Guid)p.Value) == subValue);
        }

        [Fact]
        public void SimpleSelectWithOrderBySucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  sub = '@p1'\\r\\n)\\r\\norder by\\r\\n  u.sub desc\\r\\n";
            var expected2 = "select\n  email,\n  sub as sub\nfrom\n  users as u\nwhere\n(\n  sub = '@p1'\n)\norder by\n  u.sub desc  \n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.Column("sub").Is().EqualTo(subValue);
                })
                .OrderBy("sub", "u", SortOrder.Descending)
                .Select(c => new
                {
                    email = c.Is("email"),
                    id = c.Is("sub")
                });

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }
    }
}

