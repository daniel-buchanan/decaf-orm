using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task AsEnumerableAsyncDoesntThrow()
        {
            // Act
            Func<Task<IEnumerable<dynamic>>> method = () => this.execute.AsEnumerableAsync();

            // Assert
            await method.Should().NotThrowAsync();
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
        public async Task AsEnumerableTypedAsyncDoesntThrow()
        {
            // Act
            Func<Task<IEnumerable<Person>>> method = () => this.execute.AsEnumerableAsync<Person>();

            // Assert
            await method.Should().NotThrowAsync();
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
        public async Task ToListAsyncDoesntThrow()
        {
            // Act
            Func<Task<IList<dynamic>>> method = () => this.execute.ToListAsync();

            // Assert
            await method.Should().NotThrowAsync();
        }

        [Fact]
        public void ToListTypedDoesntThrow()
        {
            // Act
            Action method = () => this.execute.ToList<Person>();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public async Task ToListTypedAsyncDoesntThrow()
        {
            // Act
            Func<Task<IList<Person>>> method = () => this.execute.ToListAsync<Person>();

            // Assert
            await method.Should().NotThrowAsync();
        }

        [Fact]
        public void ExecuteDoesntThrow()
        {
            // Act
            Action method = () => this.execute.Execute();

            // Assert
            method.Should().NotThrow();
        }

        [Fact]
        public async Task ExecuteAsyncDoesntThrow()
        {
            // Act
            Func<Task> method = () => this.execute.ExecuteAsync();

            // Assert
            await method.Should().NotThrowAsync();
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

