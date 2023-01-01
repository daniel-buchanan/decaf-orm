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
			var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(sub = '@p1')\\r\\n";
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
				p => p.Key == "p1" && ((Guid)p.Value) == subValue);
        }

        [Fact]
        public void SimpleSelectWithOrderBySucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(sub = '@p1')\\r\\norder by\\r\\n  u.sub desc\\r\\n";
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

        [Fact]
        public void SelectWithLikeSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(sub like '%@p1%')\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.Column("sub").Is().Like("bob");
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
        public void SelectWithStartsWithSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(sub like '@p1%')\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.Column("sub").Is().StartsWith("bob");
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
        public void SelectWithEndsWithSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(sub like '%@p1')\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.Column("sub").Is().EndsWith("bob");
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
        public void SelectWithMultipleConditionsSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  (sub = '@p1')\\r\\n  and\\r\\n  (email like '%@p2')\\r\\n)\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.And(ba =>
                    {
                        ba.Column("sub").Is().EqualTo("bob");
                        ba.Column("email").Is().EndsWith(".com");
                    });
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
        public void SelectWithManyConditionsSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  email,\\r\\n  sub as sub\\r\\nfrom\\r\\n  users as u\\r\\nwhere\\r\\n(\\r\\n  (sub = '@p1')\\r\\n  and\\r\\n  (email like '%@p2')\\r\\n  and\\r\\n  (\\r\\n    (\\r\\n      not\\r\\n      (id = @p3)\\r\\n    )\\r\\n    or\\r\\n    (sub like '%@p4')\\r\\n  )\\r\\n)\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            var subValue = Guid.NewGuid();

            // Act
            var q = this.query.Select()
                .From("users", "u")
                .Where(b =>
                {
                    b.And(ba =>
                    {
                        ba.Column("sub").Is().EqualTo("bob");
                        ba.Column("email").Is().EndsWith(".com");
                        ba.Or(bo =>
                        {
                            bo.Column("id").IsNot().EqualTo(0);
                            bo.Column("sub").Is().EndsWith("abc");
                        });
                    });
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
        public void SelectWithSimpleJoinSucceeds()
        {
            // Arrange
            var expected = "select\\r\\n  r.name as name,\\r\\n  u.email,\\r\\n  u.sub as sub\\r\\nfrom\\r\\n  users as u\\r\\njoin roles as r on\\r\\n  (\\r\\n    (r.user_id = u.id)\\r\\n  )\\r\\nwhere\\r\\n(id = @p1)\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);
            
            // Arrange
            var q = this.query.Select()
                .From("users", "u")
                .Join().From("users", "u").To("roles", "r").On(c =>
                {
                    c.Column("user_id", "r").Is().EqualTo().Column("id", "u");
                })
                .Where(b =>
                {
                    b.Column("id").Is().EqualTo(42);
                })
                .Select(c => new
                {
                    role = c.Is("name", "r"),
                    email = c.Is("email", "u"),
                    id = c.Is("sub", "u")
                });

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }
    }
}

