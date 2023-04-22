using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.tests.common.Mocks;
using pdq.tests.common.Models;
using pdq.services;
using pdq.state;
using pdq.state.Conditionals;
using Xunit;

namespace pdq.services.tests.Query
{
    public class QueryServiceWithKey1Tests
    {
        private readonly IService<Person, int> personService;

        public QueryServiceWithKey1Tests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o =>
            {
                o.EnableTransientTracking();
                o.OverrideDefaultLogLevel(LogLevel.Debug);
                o.UseMockDatabase();
            });
            services.AddPdqService<Person, int>().AsScoped();

            var provider = services.BuildServiceProvider();
            this.personService = provider.GetService<IService<Person, int>>();
        }

        [Fact]
        public void GetAllDoesNotThrow()
        {
            // Arrange

            // Act
            Action method = () =>
            {
                this.personService.All();
            };

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void GetAllContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.All();

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
        }

        [Fact]
        public void GetContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.Get(p => p.Id == 42);

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
            var where = selectContext.WhereClause as IColumn;
            where.EqualityOperator.Should().Be(EqualityOperator.Equals);
            where.Value.Should().Be(42);
        }

        [Fact]
        public void GetByIdContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.Get(42);

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
            var where = (IInValues)selectContext.WhereClause;
            where.Should().NotBeNull();
            where.GetValues().Should().Contain(42);
        }

        [Fact]
        public void GetByIdArrayContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.Get(new[] { 42, 43 });

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
            var where = (IInValues)selectContext.WhereClause;
            where.Should().NotBeNull();
            where.GetValues().Should().Satisfy(
                c => ((int)c) == 42,
                c => ((int)c) == 43);
        }

        [Fact]
        public void GetByIdEnumerableContextIsCorrect()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.PreExecution += (sender, args) =>
            {
                context = args.Context;
            };

            // Act
            this.personService.Get(new List<int> { 42, 43 });

            // Assert
            context.Should().NotBeNull();
            var selectContext = context as ISelectQueryContext;
            selectContext.QueryTargets.Should().HaveCount(1);
            selectContext.Columns.Should().Satisfy(
                c => c.Name.Equals(nameof(Person.Id)),
                c => c.Name.Equals(nameof(Person.FirstName)),
                c => c.Name.Equals(nameof(Person.LastName)),
                c => c.Name.Equals(nameof(Person.Email)),
                c => c.Name.Equals(nameof(Person.CreatedAt)),
                c => c.Name.Equals(nameof(Person.AddressId)));
            var where = (IInValues)selectContext.WhereClause;
            where.Should().NotBeNull();
            where.GetValues().Should().Satisfy(
                c => ((int)c) == 42,
                c => ((int)c) == 43);
        }
    }
}