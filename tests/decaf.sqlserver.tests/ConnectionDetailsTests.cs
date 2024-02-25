using System;
using System.Collections.Generic;
using decaf.common.Connections;
using decaf.common.Exceptions;
using FluentAssertions;
using Xunit;

namespace decaf.sqlserver.tests
{
	public class ConnectionDetailstests : SqlServerTest
	{
		[Theory]
		[MemberData(nameof(InvalidConnectionStrings))]
		public void InvalidConnectionStringFails(string connectionString)
		{
			// Act
			Action method = () => SqlServerConnectionDetails.FromConnectionString(connectionString);

			// Assert
			method.Should().Throw<ConnectionStringParsingException>();
		}

		[Theory]
		[MemberData(nameof(ValidConnectionStrings))]
		public void ValidConnectionStringPasses(
			string host,
			int port,
			string database,
			string username,
			string password)
		{
			// Arrange
			var connString = $"Server={host},{port};Database={database};User ID={username};Password={password};";

			// Act
			Func<ISqlServerConnectionDetails> func = () => SqlServerConnectionDetails.FromConnectionString(connString);

			// Assert
			func.Should().NotThrow<ConnectionStringParsingException>();

			var details = func();
			details.Hostname.Should().Be(host);
			details.Port.Should().Be(port);
			details.DatabaseName.Should().Be(database);

			var auth = details.Authentication;
			auth.AuthenticationType.Should().Be(ConnectionAuthenticationType.UsernamePassword);

			var passAuth = auth as UsernamePasswordAuthentication;
			passAuth.Should().NotBeNull();
			passAuth.Username.Should().Be(username);
			passAuth.Password.Should().Be(password);
		}

		public static IEnumerable<object[]> InvalidConnectionStrings
		{
			get
			{
                yield return new object[] { null };
                yield return new object[] { string.Empty };
				yield return new object[] { "Bob=Hello;" };
				yield return new object[] { "Host=bob;" };
                yield return new object[] { "Host=bob;Port=5432;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;Username=admin;" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgres;Username=admin" };
                yield return new object[] { "Host=bob;Port=5432;Database=postgresUsername=admin;" };
            }
		}

		public static IEnumerable<object[]> ValidConnectionStrings
		{
			get
			{
				yield return new object[] { "localhost", 5432, "postgres", "admin", "postgres" };
                yield return new object[] { "cx45.internal", 1234, "postgres", "admin", "postgres" };
                yield return new object[] { "cx45.internal", 1234, "postgres", "admin", "Password.123856" };
            }
		}
	}
}

