using System;
using System.Collections.Generic;
using FluentAssertions;
using pdq.common;
using pdq.common.Utilities.Reflection.Dynamic;
using pdq.common.ValueFunctions;
using pdq.core_tests.Models;
using pdq.state;
using pdq.state.Utilities;
using Xunit;

namespace pdq.core_tests.Helpers
{
    public class DynamicPropertyInfoTests
    {
        public DynamicPropertyInfoTests()
        {
        }

        [Fact]
        public void SetNameSucceeds()
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetName("bob");

            // Assert
            info.Name.Should().NotBeNullOrWhiteSpace();
            info.Name.Should().Be("bob");
        }

        [Fact]
        public void SetNewNameSucceeds()
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetNewName("bob");

            // Assert
            info.NewName.Should().NotBeNullOrWhiteSpace();
            info.NewName.Should().Be("bob");
        }

        [Fact]
        public void SetAliasSucceeds()
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetAlias("b");

            // Assert
            info.Alias.Should().NotBeNullOrWhiteSpace();
            info.Alias.Should().Be("b");
        }

        [Fact]
        public void SetTypeSucceeds()
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetType(typeof(Person));

            // Assert
            info.Type.Should().NotBeNull();
            info.Type.Should().Be(typeof(Person));
        }

        [Theory]
        [MemberData(nameof(ValidValues))]
        public void SetValueSucceeds(object value)
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetValue(value);

            // Assert
            info.Value.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(ValidFunctions))]
        public void SetFunctionSucceeds(IValueFunction function)
        {
            // Arrange
            var info = DynamicColumnInfo.Create();

            // Act
            info.SetFunction(function);

            // Assert
            info.Function.Should().Be(function);
        }

        [Theory]
        [MemberData(nameof(EqualColumns))]
        public void ColumnsAreEqual(DynamicColumnInfo a, DynamicColumnInfo b)
        {
            // Act
            var eq = a == b;

            // Assert
            eq.Should().Be(true);
        }

        [Theory]
        [MemberData(nameof(EqualColumns))]
        public void ColumnsAreEqual_CompareTo(DynamicColumnInfo a, DynamicColumnInfo b)
        {
            // Act
            var eq = a.CompareTo(b);

            // Assert
            eq.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(NotEqualColumns))]
        public void ColumnsAreNotEqual_CompareTo(DynamicColumnInfo a, DynamicColumnInfo b)
        {
            // Arrange
            var left = a;
            var right = b;
            if (left == null)
            {
                left = b;
                right = a;
            }

            // Act
            var eq = left.CompareTo(right);

            // Assert
            eq.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(NotEqualColumns))]
        public void ColumnsAreNotEqual(DynamicColumnInfo a, DynamicColumnInfo b)
        {
            // Act
            var eq = a != b;

            // Assert
            eq.Should().Be(true);
        }

        [Theory]
        [MemberData(nameof(EqualColumns))]
        public void HashCodesAreSame(DynamicColumnInfo a, DynamicColumnInfo b)
        {
            // Act
            var hashCodeA = a.GetHashCode();
            var hashCodeB = b.GetHashCode();
            var eq = hashCodeA == hashCodeB;

            // Assert
            eq.Should().Be(true);
        }

        public static IEnumerable<object[]> EqualColumns
        {
            get
            {
                yield return new object[] { DynamicColumnInfo.Create(), DynamicColumnInfo.Create() };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob"), DynamicColumnInfo.Create(name: "Bob") };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob", newName: "Andy"), DynamicColumnInfo.Create(name: "Bob", newName: "Andy") };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob", newName: "Andy", value: 42), DynamicColumnInfo.Create(name: "Bob", newName: "Andy", value: 42) };
            }
        }

        public static IEnumerable<object[]> NotEqualColumns
        {
            get
            {
                yield return new object[] { DynamicColumnInfo.Create(), null };
                yield return new object[] { null, DynamicColumnInfo.Create() };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob"), DynamicColumnInfo.Create(name: "James") };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob", newName: "Andy"), DynamicColumnInfo.Create(name: "James", newName: "Andy") };
                yield return new object[] { DynamicColumnInfo.Create(name: "Bob", newName: "Andy", value: 42), DynamicColumnInfo.Create(name: "Bob", newName: "James", value: 42) };
            }
        }

        public static IEnumerable<object[]> ValidValues
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { 42 };
                yield return new object[] { true };
                yield return new object[] { false };
                yield return new object[] { 42D };
                yield return new object[] { 42M };
                yield return new object[] { 42F };
                yield return new object[] { "hello world" };
            }
        }

        public static IEnumerable<object[]> ValidFunctions
        {
            get
            {
                yield return new object[] { StringContains.Create("smith") };
                yield return new object[] { ToLower.Create() };
                yield return new object[] { ToUpper.Create() };
                yield return new object[] { Substring.Create(0) };
                yield return new object[] { Substring.Create(0, 9) };
            }
        }
    }
}

