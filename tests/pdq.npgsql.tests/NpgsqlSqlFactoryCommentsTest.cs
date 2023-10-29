using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using pdq.common;
using pdq.common.Connections;
using pdq.common.Utilities;
using pdq.core_tests.Helpers;
using pdq.tests.common.Mocks;
using pdq.tests.common.Models;
using pdq.services;
using Xunit;

namespace pdq.npgsql.tests
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
            services.AddPdqService<Person>().AsScoped();

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
            lines[0].Should().StartWith("-- pdq :: query hash:");
            lines[1].Should().StartWith("-- pdq :: generated at:");
        }
    }
}

