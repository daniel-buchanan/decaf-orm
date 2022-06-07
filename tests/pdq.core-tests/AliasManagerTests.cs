using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;
using Xunit;

namespace pdq.core_tests
{
	public class AliasManagerTests
	{
		private readonly IAliasManager aliasManager;

		public AliasManagerTests()
		{
			this.aliasManager = new AliasManager();
		}

        [Theory]
        [MemberData(nameof(AddTests))]
		public void AddSucceeds(string alias, string name, bool aliasIsNull, string expected)
        {
			// Arrange

			// Act
			var addedAlias = this.aliasManager.Add(alias, name);

			// Assert
			if (aliasIsNull) Assert.Equal(expected, addedAlias);
			else Assert.Equal(alias, addedAlias);
        }

		[Theory]
		[MemberData(nameof(AddTests))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Same Test set, but not using all params")]
        public void AddSucceedsAndContainsOnlyOne(string alias, string name, bool aliasIsNull, string expected)
		{
			// Arrange
			this.aliasManager.Add(alias, name);

			// Act
			var aliases = this.aliasManager.All();

			// Assert
			Assert.Single(aliases);
		}

		[Theory]
		[MemberData(nameof(AddTests))]
		public void GetByAssociationSucceeds(string alias, string name, bool aliasIsNull, string expected)
		{
			// Arrange
			this.aliasManager.Add(alias, name);

			// Act
			var foundAlias = this.aliasManager.FindByAssociation(name);

			// Assert
			if (aliasIsNull) Assert.NotNull(foundAlias);
			else Assert.Equal(expected, alias);
		}

		public static IEnumerable<object[]> AddTests
        {
			get
            {
				yield return new object[] { null, "test", true, "t0" };
				yield return new object[] { null, "hello", true, "h0" };
				yield return new object[] { null, "bob", true, "b0" };
				yield return new object[] { "ts", "test", false, "ts" };
			}
        }
	}
}

