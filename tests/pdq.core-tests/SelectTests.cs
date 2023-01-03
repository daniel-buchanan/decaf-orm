using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Conditionals;
using pdq.state.Conditionals.ValueFunctions;
using pdq.state.QueryTargets;
using Xunit;

namespace pdq.core_tests
{
    public class SelectTests
    {
        private IQueryInternal query;

        public SelectTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            this.query = transient.Query() as IQueryInternal;
        }

        [Fact]
        public void SimpleSelectSucceeds()
        {
            // Act
            this.query
                .Select()
                .From<Person>(p => p)
                .Where(p => p.LastName.Contains("smith"));

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(1);
            var where = context.WhereClause as IColumn;
            where.Should().NotBeNull();
            where.Details.Name.Should().Be(nameof(Person.LastName));
            where.Details.Source.Alias.Should().Be("p");
            where.ValueFunction.Should().BeAssignableTo(typeof(StringContains));
        }

        [Fact]
        public void SelectWithMultipleTablesSucceeds()
        {
            // Act
            this.query
                .Select()
                .From<Person>(p => p)
                .From<Address>(a => a)
                .Where((p, a) => p.LastName.Contains("smith"));

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(2);
            var where = context.WhereClause as IColumn;
            where.Should().NotBeNull();
            where.Details.Name.Should().Be(nameof(Person.LastName));
            where.Details.Source.Alias.Should().Be("p");
            where.ValueFunction.Should().BeAssignableTo(typeof(StringContains));
        }

        [Fact]
        public void SimpleJoinSucceeds()
        {
            // Act
            this.query
                .Select()
                .From("users", "u")
                .Join()
                    .From("users", "u")
                    .To("note", "n")
                    .On(b =>
                    {
                        b.Column("id", "u").Is().EqualTo().Column("id", "n");
                    })
                .Where(b =>
                {
                    b.Column("value", "n").Is().Null();
                });

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(2);
            context.Joins.Should().HaveCount(1);
            var whereClause = context.WhereClause as IColumn;
            whereClause.Should().NotBeNull();
        }

        [Fact]
        public void SimpleSelectWithJoinSucceeds()
        {
            // Act
            this.query
                .Select()
                .From<Person>(p => p)
                .Join<Address>((p, a) => p.AddressId == a.Id)
                .Where((p, a) => p.LastName.Contains("smith") && a.PostCode == "3216");

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(2);
            context.Joins.Should().HaveCount(1);
            context.WhereClause.Should().BeAssignableTo(typeof(state.Conditionals.And));
        }

        [Theory]
        [MemberData(nameof(DateTimeExtensionTests))]
        public void SimpleSelectDateTimeSucceeds(Expression<Func<Person, Address, Result>> selectExpression)
        {
            // Act
            this.query
                .Select()
                .From<Person>(p => p)
                .Join<Address>((p, a) => p.AddressId == a.Id)
                .Where((p, a) => p.LastName.Contains("smith") && a.PostCode == "3216")
                .Select(selectExpression);

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(2);
            context.Joins.Should().HaveCount(1);
            context.WhereClause.Should().BeAssignableTo(typeof(state.Conditionals.And));
        }

        [Fact]
        public void SelectAllSucceeds()
        {
            // Arrange

            // Act
            this.query
                .Select()
                .From("person", "p")
                .Where(b => b.Column("id", "p").Is().EqualTo(42))
                .SelectAll<Person>("p");

            // Assert
            var context = this.query.Context as ISelectQueryContext;
            context.QueryTargets.Should().HaveCount(1);
            context.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
        }

        public static IEnumerable<object[]> DateTimeExtensionTests
        {
            get
            {
                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Year)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Day)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Month)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Minute)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Second)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Millisecond)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.DatePart(common.DatePart.Epoch)
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Year()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Month()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Day()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Minute()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Second()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Millisecond()
                })};

                yield return new object[] { ToExpression((p, a) => new Result {
                    City = a.City,
                    Name = p.FirstName,
                    Timestamp = p.CreatedAt.Epoch()
                })};
            }
        }

        private static Expression<Func<Person, Address, Result>> ToExpression(Expression<Func<Person, Address, Result>> expression)
            => expression;
    }
}

