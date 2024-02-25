using System;
using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Connections;
using decaf.services;
using decaf.tests.common.Mocks;
using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using decaf.common.Utilities;
using decaf.core_tests.Helpers;
using Xunit;

namespace decaf.npgsql.tests
{
    public class NpgsqlSqlFactoryCommentTest : NpgsqlTest
    {
        private readonly ISqlFactory sqlFactory;
        private readonly IService<Person> personService;

        public NpgsqlSqlFactoryCommentTest() : base(false)
        {
            services.Replace<IConnectionFactory, MockConnectionFactory>();
            services.Replace<ITransactionFactory, MockTransactionFactory>();
            services.AddScoped<IConnectionDetails>(s => new MockConnectionDetails());
            services.AddDecafService<Person>().AsScoped();

            BuildServiceProvider();

            this.sqlFactory = provider.GetService<ISqlFactory>();
            this.personService = provider.GetService<IService<Person>>();
        }

        [Fact]
        public void QueryHeadersPresent()
        {
            // Arrange
            IQueryContext context = null;
            this.personService.OnBeforeExecution += (sender, args) =>
            {
                context = args.Context;
            };
            this.personService.Find(p => p.Email == "bob@bob.com");

            // Act
            var template = this.sqlFactory.ParseTemplate(context);
            var sql = template.Sql;

            // Assert
            var lines = sql.Split(Environment.NewLine);
            lines[0].Should().StartWith("-- decaf :: query hash:");
            lines[1].Should().StartWith("-- decaf :: generated at:");
        }
    }
}

