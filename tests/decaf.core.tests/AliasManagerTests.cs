﻿using System.Collections.Generic;
using System.Linq;
using decaf.common;
using decaf.common.Utilities;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests;

public class AliasManagerTests
{
	private readonly IAliasManager aliasManager;

	public AliasManagerTests()
	{
		aliasManager = AliasManager.Create();
	}

	[Theory]
	[MemberData(nameof(AddTests))]
	public void AddSucceeds(string alias, string name, string expected)
	{
		// Arrange

		// Act
		var addedAlias = aliasManager.Add(alias, name);

		// Assert
		addedAlias.Should().BeEquivalentTo(expected);
	}

	[Theory]
	[MemberData(nameof(AddTests))]
	public void AddSucceedsAndContainsOnlyOne(string alias, string name, string expected)
	{
		// Arrange
		var expectedAlias = ManagedAlias.Create(expected, name);
		aliasManager.Add(alias, name);

		// Act
		var aliases = aliasManager.All();

		// Assert
		aliases.Should().ContainSingle();
		aliases.Should().ContainEquivalentOf(expectedAlias);
	}

	[Theory]
	[MemberData(nameof(AddTests))]
	public void GetByAssociationSucceeds(string alias, string name, string expected)
	{
		// Arrange
		var expectedAlias = ManagedAlias.Create(expected, name);
		aliasManager.Add(alias, name);

		// Act
		var foundAlias = aliasManager.FindByAssociation(name);

		// Assert
		foundAlias.Should().ContainSingle();
		foundAlias.First().Should().BeEquivalentTo(expectedAlias);
	}

	public static IEnumerable<object[]> AddTests
	{
		get
		{
			yield return new object[] { null, "test", "t0" };
			yield return new object[] { null, "hello", "h0" };
			yield return new object[] { null, "bob", "b0" };
			yield return new object[] { "ts", "test", "ts" };
		}
	}
}