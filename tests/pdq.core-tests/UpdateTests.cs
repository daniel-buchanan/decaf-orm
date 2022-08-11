using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Conditionals;
using Xunit;

namespace pdq.core_tests
{
    public class UpdateTests
    {
        private IQueryInternal query;

        public UpdateTests()
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
        public void SimpleUpdateSucceeds()
        {
            // Act
            this.query
                .Update()
                .Table<Person>()
                .Set(p => p.Email, "bob@bob.com")
                .Where(p => p.Id == 42);

            // Assert
            var context = this.query.Context as IUpdateQueryContext;
            context.QueryTargets.Should().HaveCount(1);
            context.Updates.Should().HaveCount(1);
            var where = context.WhereClause as IColumn;
            where.Should().NotBeNull();
            where.Details.Name.Should().Be(nameof(Person.Id));
            where.Details.Source.Alias.Should().Be("p0");
            where.Value.Should().BeEquivalentTo(42);
        }

        [Fact]
        public void SimpleUpdateWithAliasSucceeds()
        {
            // Act
            this.query
                .Update()
                .Table<Person>(p => p)
                .Set(p => p.Email, "bob@bob.com")
                .Where(p => p.Id == 42);

            // Assert
            var context = this.query.Context as IUpdateQueryContext;
            context.QueryTargets.Should().HaveCount(1);
            context.Updates.Should().HaveCount(1);
            var where = context.WhereClause as IColumn;
            where.Should().NotBeNull();
            where.Details.Name.Should().Be(nameof(Person.Id));
            where.Details.Source.Alias.Should().Be("p");
            where.Value.Should().BeEquivalentTo(42);
        }

        [Fact]
        public void SimpleUpdateWithConcreteSetSucceeds()
        {
            // Act
            this.query
                .Update()
                .Table<Person>(p => p)
                .Set(new Person
                {
                    LastName = "Smith"
                })
                .Where(p => p.Id == 42);

            // Assert
            var context = this.query.Context as IUpdateQueryContext;
            context.QueryTargets.Should().HaveCount(1);
            context.Updates.Should().HaveCount(1);
            var where = context.WhereClause as IColumn;
            where.Should().NotBeNull();
            where.Details.Name.Should().Be(nameof(Person.Id));
            where.Details.Source.Alias.Should().Be("p");
            where.Value.Should().BeEquivalentTo(42);
        }
    }
}

