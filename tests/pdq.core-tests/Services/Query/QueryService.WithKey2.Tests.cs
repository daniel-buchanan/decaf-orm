using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.services;
using pdq.state;
using pdq.state.Conditionals;
using Xunit;

namespace pdq.core_tests.Services.Query
{
    public class QueryServiceWithKey2Tests
    {
        private readonly IQuery<Address, int, int> addressService;

        public QueryServiceWithKey2Tests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddPdqService<Address, int, int>().AsScoped();
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();

            var provider = services.BuildServiceProvider();
            this.addressService = provider.GetService<IQuery<Address, int, int>>();
        }

        [Fact]
        public void GetAllDoesNotThrow()
        {
            // Arrange

            // Act
            Action method = () =>
            {
                this.addressService.All();
            };

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void GetAllContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.addressService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.addressService.All();

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Address.Id)),
                c => c.Name.Equals(nameof(Address.PersonId)),
                c => c.Name.Equals(nameof(Address.Line1)),
                c => c.Name.Equals(nameof(Address.Line2)),
                c => c.Name.Equals(nameof(Address.Line3)),
                c => c.Name.Equals(nameof(Address.Line4)),
                c => c.Name.Equals(nameof(Address.City)),
                c => c.Name.Equals(nameof(Address.PostCode)),
                c => c.Name.Equals(nameof(Address.Region)),
                c => c.Name.Equals(nameof(Address.Country)));
        }

        [Fact]
        public void GetContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.addressService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.addressService.Get(p => p.Id == 42);

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Address.Id)),
                c => c.Name.Equals(nameof(Address.PersonId)),
                c => c.Name.Equals(nameof(Address.Line1)),
                c => c.Name.Equals(nameof(Address.Line2)),
                c => c.Name.Equals(nameof(Address.Line3)),
                c => c.Name.Equals(nameof(Address.Line4)),
                c => c.Name.Equals(nameof(Address.City)),
                c => c.Name.Equals(nameof(Address.PostCode)),
                c => c.Name.Equals(nameof(Address.Region)),
                c => c.Name.Equals(nameof(Address.Country)));
            var where = selectContext.WhereClause as IColumn;
            where.EqualityOperator.Should().Be(EqualityOperator.Equals);
            where.Value.Should().Be(42);
        }

        [Fact]
        public void GetByIdContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.addressService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.addressService.Get(1, 42);

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Address.Id)),
                c => c.Name.Equals(nameof(Address.PersonId)),
                c => c.Name.Equals(nameof(Address.Line1)),
                c => c.Name.Equals(nameof(Address.Line2)),
                c => c.Name.Equals(nameof(Address.Line3)),
                c => c.Name.Equals(nameof(Address.Line4)),
                c => c.Name.Equals(nameof(Address.City)),
                c => c.Name.Equals(nameof(Address.PostCode)),
                c => c.Name.Equals(nameof(Address.Region)),
                c => c.Name.Equals(nameof(Address.Country)));
            var where = selectContext.WhereClause as state.Conditionals.And;
            where.Children.Should().HaveCount(2);
        }
    }
}

