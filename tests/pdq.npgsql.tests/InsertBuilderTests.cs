using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using Xunit;

namespace pdq.npgsql.tests
{
	public class InsertBuilderTests
	{
		private readonly IQuery query;

		public InsertBuilderTests()
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
		public void InsertSucceeds()
		{
            // Arrange
            var expected = "insert into\\r\\n  users\\r\\n  (first_name,last_name)\\r\\nvalues\\r\\n('bob','smith')\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);

            // Act
            var q = this.query.Insert()
				.Into("users")
				.Columns(c => new
				{
					first_name = c.Is<string>(),
					last_name = c.Is<string>()
				})
				.Values(new[]
				{
					new
					{
						first_name = "bob",
						last_name = "smith"
					}
				});

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }

        [Fact]
        public void InsertMultipleValuesSucceeds()
        {
            // Arrange
            var expected = "insert into\\r\\n  users\\r\\n  (first_name,last_name)\\r\\nvalues\\r\\n('bob','smith'),\\r\\n('james','smith')\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);

            // Act
            var q = this.query.Insert()
                .Into("users")
                .Columns(c => new
                {
                    first_name = c.Is<string>(),
                    last_name = c.Is<string>()
                })
                .Values(new[]
                {
                    new
                    {
                        first_name = "bob",
                        last_name = "smith"
                    },
                    new
                    {
                        first_name = "james",
                        last_name = "smith"
                    }
                });

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }

        [Fact]
        public void InsertFromQuerySucceeds()
        {
            // Arrange
            var expected = "insert into\\r\\n  users\\r\\n  (first_name,last_name)\\r\\nselect\\r\\n  t.first_name,\\r\\n  t.last_name\\r\\nfrom\\r\\n  temp as t\\r\\nwhere\\r\\n(t.id = @p1)\\r\\n";
            expected = expected.Replace("\\r\\n", Environment.NewLine);

            // Act
            var q = this.query.Insert()
                .Into("users")
                .Columns(c => new
                {
                    first_name = c.Is<string>(),
                    last_name = c.Is<string>()
                })
                .From(q =>
                {
                    q.From("temp", "t")
                        .Where(b => b.Column("id", "t").Is().EqualTo(42))
                        .Select(b => new
                        {
                            first_name = b.Is("first_name", "t"),
                            last_name = b.Is("last_name", "t")
                        });
                });

            var sql = q.GetSql();

            // Assert
            sql.Should().Be(expected);
        }
    }
}

