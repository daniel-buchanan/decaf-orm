using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using pdq.common;
using pdq.common.Logging;
using pdq.core_tests.Mocks;
using pdq.core_tests.Models;
using pdq.Implementation;
using pdq.state;
using Xunit;

namespace pdq.core_tests
{
    public class ExecuteTests
    {
        private readonly IExecute execute;
        private readonly ISelectQueryContext context;

        public ExecuteTests()
        {
            var services = new ServiceCollection();
            services.AddPdq(o => o.UseMockDatabase());
            services.AddScoped<IConnectionDetails, MockConnectionDetails>();
            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IUnitOfWork>();
            var transient = uow.Begin();
            var query = transient.Query() as IQueryInternal;
            var sqlFactory = provider.GetService<ISqlFactory>();
            this.context = query.CreateContext<ISelectQueryContext>();
            this.execute = new InheritedExecutor<ISelectQueryContext>(query, this.context, sqlFactory);
        }

        [Fact]
        public void DynamicReturnsSelf()
        {
            // Act
            var dyn = this.execute.Dynamic();

            // Assert
            dyn.Should().Be(this.execute);
        }

        [Fact]
        public void AsEnumerableDoesntThrow()
        {
            // Act
            Action method = () => this.execute.AsEnumerable();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void AsEnumerableTypedDoesntThrow()
        {
            // Act
            Action method = () => this.execute.AsEnumerable<Person>();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void ToListDoesntThrow()
        {
            // Act
            Action method = () => this.execute.ToList();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public void ToListTypedDoesntThrow()
        {
            // Act
            Action method = () => this.execute.ToList<Person>();

            // Assert
            method.Should().NotThrow();
        }
    }

    class InheritedExecutor<T> : Execute<T>
        where T : IQueryContext
    {
        public InheritedExecutor(IQueryInternal query, T context, ISqlFactory sqlFactory)
            : base(query, context, sqlFactory)
        {
        }
    }
}

