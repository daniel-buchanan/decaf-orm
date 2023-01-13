using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using Xunit;

namespace pdq.npgsql.tests
{
	public class DeleteBuilderTests
	{
		private readonly IQuery query;

		public DeleteBuilderTests()
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
		public void SimpleDeleteSucceeds()
		{
			// Arrange
			var expected = "delete from\\r\\n  users as u\\r\\nwhere\\r\\n(u.sub = '@p1')\\r\\nreturning\\r\\n  deleted.id\\r\\n";
			expected = expected.Replace("\\r\\n", Environment.NewLine);
			var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b => b.Column("sub", "u").Is().EqualTo(subValue))
                .Output("id");

			var sql = q.GetSql();

			// Assert
			sql.Should().Be(expected);
		}

        [Fact]
        public void SimpleDeleteReturnsCorrectParameters()
        {
            // Arrange
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b => b.Column("sub", "u").Is().EqualTo(subValue))
                .Output("id");

            var parameters = q.GetSqlParameters();

			// Assert
			parameters.Should().Satisfy(
				p => p.Key == "p1" && ((Guid)p.Value) == subValue);
        }

        [Fact]
        public void DeleteWithLikeSucceeds()
        {
            // Arrange
            var expected = "delete from\\r\\n  users as u\\r\\nwhere\\r\\n(u.sub like '%@p1%')\\r\\nreturning\\r\\n  deleted.id\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b => b.Column("sub", "u").Is().Like("bob"))
                .Output("id");

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }

        [Fact]
        public void DeleteWithStartsWithSucceeds()
        {
            // Arrange
            var expected = "delete from\\r\\n  users as u\\r\\nwhere\\r\\n(u.sub like '@p1%')\\r\\nreturning\\r\\n  deleted.id\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b => b.Column("sub", "u").Is().StartsWith("bob"))
                .Output("id");

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }

        [Fact]
        public void DeleteWithEndsWithSucceeds()
        {
            // Arrange
            var expected = "delete from\\r\\n  users as u\\r\\nwhere\\r\\n(u.sub like '%@p1')\\r\\nreturning\\r\\n  deleted.id\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b => b.Column("sub", "u").Is().EndsWith("bob"))
                .Output("id");

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }

        [Fact]
        public void DeleteWithMultipleConditionsSucceeds()
        {
            // Arrange
            var expected = "delete from\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  (u.sub = '@p1')\\r\\n  and\\r\\n  (u.email like '%@p2')\\r\\n)\\r\\nreturning\\r\\n  deleted.id\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Delete()
                .From("users", "u")
                .Where(b =>
                {
                    b.And(ba =>
                    {
                        ba.Column("sub", "u").Is().EqualTo("bob");
                        ba.Column("email", "u").Is().EndsWith(".com");
                    });
                })
                .Output("id");

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }
    }
}

