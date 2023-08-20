using System;
using System.Collections.Generic;
using FluentAssertions;
using pdq.common.Connections;
using pdq.common.Exceptions;
using Xunit;

namespace pdq.npgsql.tests
{
	public class ConnectionDetailstests : NpgsqlTest
	{
		[Theory]
		[MemberData(nameof(InvalidConnectionStrings))]
		public void InvalidConnectionStringFails(string connectionString)
		{
			// Act
			Action method = () => NpgsqlConnectionDetails.FromConnectionString(connectionString);

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
			var connString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

			// Act
			Func<INpgsqlConnectionDetails> func = () => NpgsqlConnectionDetails.FromConnectionString(connString);

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

