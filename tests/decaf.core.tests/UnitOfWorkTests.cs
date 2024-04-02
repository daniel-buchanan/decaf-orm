using System;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Exceptions;
using decaf.tests.common.Mocks;
using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.core_tests;

public class UnitOfWorkTests : CoreTestBase
{
    [Fact]
    public void UnitOfWorkFluentSuccess()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var hasException = false;
        User result = null;

        // Act
        unit.OnException(_ => hasException = true)
            .OnSuccess(() => hasException = false)
            .Query(q =>
            {
                result = q.Select()
                    .From<User>(u => u)
                    .Where(u => u.Id == 1)
                    .SelectAll<User>(u => u)
                    .SingleOrDefault();
            })
            .PersistChanges();

        // Assert
        hasException.Should().BeFalse();
    }

    [Fact]
    public void CreateUnitSucceeds()
    {
        // Arrange
        var factory = provider.GetService<IUnitOfWorkFactory>();

        // Act
        Action method = () => factory.Create();

        // Assert
        method.Should().NotThrow();
    }
    
    [Fact]
    public void CreateUnitWithDetailsSucceeds()
    {
        // Arrange
        var factory = provider.GetService<IUnitOfWorkFactory>();

        // Act
        Action method = () => factory.Create(new MockConnectionDetails());

        // Assert
        method.Should().NotThrow();
    }

    [Fact]
    public void WarnWhenNotAllQueriesExecuted()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();

        // Act
        unit.Query(q =>
            {
                var result = q.Select()
                    .From<User>(u => u)
                    .Where(u => u.Id == 1)
                    .SelectAll<User>(u => u)
                    .SingleOrDefault();
            })
            .Query(q =>
            {
                q.Delete()
                    .From<User>()
                    .Where(u => u.Id == 42);
            })
            .PersistChanges();

        // Assert
        logger.Invocations.Should().Contain(i => i.Method.Name == "Warning");
    }

    [Fact]
    public void RollbackSucceeds()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var exceptionThrown = false;

        // Act
        unit.OnException(_ =>
            {
                exceptionThrown = true;
                return false;
            })
            .Query(q => throw new Exception("testing rollback"))
            .PersistChanges();

        // Assert
        exceptionThrown.Should().Be(true);
    }

    [Fact]
    public void CommitThrowsException()
    {
        // Arrange
        var p = Build(b =>
        {
            b.ThrowOnCommit();
        });
        var decaf = p.GetService<IDecaf>();
        var unit = decaf.BuildUnit();

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .PersistChanges();

        // Assert
        method.Should().Throw<CommitException>();
    }
    
    [Fact]
    public void RollbackThrowsException()
    {
        // Arrange
        var p = Build(b =>
        {
            b.ThrowOnCommit();
            b.ThrowOnRollback();
        });
        var decaf = p.GetService<IDecaf>();
        var unit = decaf.BuildUnit();

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .PersistChanges();

        // Assert
        method.Should().Throw<RollbackException>();
    }
    
    [Fact]
    public void SwallowExceptionsNoneThrown()
    {
        // Arrange
        var p = Build(b =>
        {
            b.ThrowOnCommit();
            b.ThrowOnRollback();
        }, swallowExceptions: true);
        var decaf = p.GetService<IDecaf>();
        var unit = decaf.BuildUnit();

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .PersistChanges();

        // Assert
        method.Should().NotThrow();
    }
    
    [Fact]
    public void SwallowExceptionsOnExceptionCalled()
    {
        // Arrange
        var p = Build(b =>
        {
            b.ThrowOnCommit();
            b.ThrowOnRollback();
        }, swallowExceptions: true);
        var decaf = p.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var exceptionHandled = false;

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .OnException(e =>
            {
                exceptionHandled = true;
            })
            .PersistChanges();

        // Assert
        method.Should().NotThrow();
        exceptionHandled.Should().BeTrue();
    }
    
    [Fact]
    public void DisposeSuccessful()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var exceptionHandled = false;

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .OnException(e =>
            {
                exceptionHandled = true;
            })
            .PersistChanges()
            .Dispose();

        // Assert
        method.Should().NotThrow();
    }
    
    [Fact]
    public void CloseOnCommitOrRollbackTriggered()
    {
        // Arrange
        var p = Build(b => b.Noop());
        var decaf = p.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var exceptionHandled = false;

        // Act
        Action method = () => unit
            .Query(q => q.Select().From<User>().Where(u => u.Id != 42))
            .OnException(e =>
            {
                exceptionHandled = true;
            })
            .PersistChanges();

        // Assert
        method.Should().NotThrow();
        var ext = unit as IUnitOfWorkExtended;
        ext.Connection.State.Should().Be(ConnectionState.Closed);
    }
}