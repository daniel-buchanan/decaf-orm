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
    public class QueryServiceWithKey3Tests
    {
        private readonly IService<AddressNote, int, int, int> addressService;

        public QueryServiceWithKey3Tests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddPdqService<AddressNote, int, int, int>().AsScoped();

            var provider = services.BuildServiceProvider();
            this.addressService = provider.GetService<IService<AddressNote, int, int, int>>();
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
                c => c.Name.Equals(nameof(AddressNote.Id)),
                c => c.Name.Equals(nameof(AddressNote.AddressId)),
                c => c.Name.Equals(nameof(AddressNote.PersonId)),
                c => c.Name.Equals(nameof(AddressNote.Value)));
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
                c => c.Name.Equals(nameof(AddressNote.Id)),
                c => c.Name.Equals(nameof(AddressNote.AddressId)),
                c => c.Name.Equals(nameof(AddressNote.PersonId)),
                c => c.Name.Equals(nameof(AddressNote.Value)));
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
            this.addressService.Get(1, 42, 43);

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(AddressNote.Id)),
                c => c.Name.Equals(nameof(AddressNote.AddressId)),
                c => c.Name.Equals(nameof(AddressNote.PersonId)),
                c => c.Name.Equals(nameof(AddressNote.Value)));
            var where = selectContext.WhereClause as state.Conditionals.And;
            where.Children.Should().HaveCount(3);
        }
    }
}

