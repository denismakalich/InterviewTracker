using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain;

public class UserTests
{
    private readonly IFixture _fixture;

    public UserTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var name = "Test User";
        var email = new Email("test@example.com");
        var roleId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(Guid.Empty, name, email, roleId));
    }

    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly_WhenValidArgumentsAreProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test User";
        var email = new Email("test@example.com");
        var roleId = Guid.NewGuid();

        // Act
        var user = new User(id, name, email, roleId);

        // Assert
        user.Id.Should().Be(id);
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
        user.RoleId.Should().Be(roleId);
    }

    [Fact]
    public void Create_Should_CreateNewUserWithNewGuidAndSetProperties()
    {
        // Arrange
        var name = "Test User";
        var email = new Email("test@example.com");
        var roleId = Guid.NewGuid();

        // Act
        var user = User.Create(name, email, roleId);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
        user.RoleId.Should().Be(roleId);
    }

    [Fact]
    public void SetName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "OldName", new Email("old.email@example.com"), Guid.NewGuid());
        var newName = "NewName";

        // Act
        user.SetName(newName);

        // Assert
        user.Name.Should().Be(newName);
    }

    [Fact]
    public void SetName_WithNullOrWhiteSpaceName_ShouldThrowArgumentException()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "OldName", new Email("old.email@example.com"), Guid.NewGuid());
        string? invalidName = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.SetName(invalidName))
            .ParamName.Should().Be("name");
    }

    [Fact]
    public void SetEmail_WithValidEmail_ShouldUpdateEmail()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "John Doe", new Email("old.email@example.com"), Guid.NewGuid());
        var newEmail = new Email("new.email@example.com");

        // Act
        user.SetEmail(newEmail);

        // Assert
        user.Email.Should().Be(newEmail);
    }

    [Fact]
    public void SetEmail_WithNullEmail_ShouldThrowArgumentNullException()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "John Doe", new Email("old.email@example.com"), Guid.NewGuid());
        Email? invalidEmail = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => user.SetEmail(invalidEmail))
            .ParamName.Should().Be("email");
    }

    [Fact]
    public void SetRole_WithValidRoleId_ShouldUpdateRoleId()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "John Doe", new Email("john.doe@example.com"), Guid.NewGuid());
        var newRoleId = Guid.NewGuid();

        // Act
        user.Role(newRoleId);

        // Assert
        user.RoleId.Should().Be(newRoleId);
    }

    [Fact]
    public void SetRole_WithEmptyRoleId_ShouldThrowArgumentException()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "John Doe", new Email("john.doe@example.com"), Guid.NewGuid());
        var invalidRoleId = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.Role(invalidRoleId))
            .ParamName.Should().Be("roleId");
    }
}