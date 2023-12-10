using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain;

public class RoleTests
{
    private readonly IFixture _fixture;

    public RoleTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var name = "Test Role";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Role(Guid.Empty, name));
    }

    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly_WhenValidArgumentsAreProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Role";

        // Act
        var role = new Role(id, name);

        // Assert
        role.Id.Should().Be(id);
        role.Name.Should().Be(name);
    }

    [Fact]
    public void Create_Should_CreateRoleWithNewGuidAndSetName()
    {
        // Arrange
        var name = "Test Role";

        // Act
        var role = Role.Create(name);

        // Assert
        role.Id.Should().NotBeEmpty();
        role.Name.Should().Be(name);
    }

    [Fact]
    public void SetName_Should_ThrowArgumentException_WhenNameIsNullOrWhiteSpace()
    {
        // Arrange
        var role = _fixture.Create<Role>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => role.SetName(null));
        Assert.Throws<ArgumentException>(() => role.SetName(""));
        Assert.Throws<ArgumentException>(() => role.SetName(" "));
    }

    [Fact]
    public void SetName_Should_SetPropertyNameCorrectly_WhenValidNameIsProvided()
    {
        // Arrange
        var role = _fixture.Create<Role>();
        var newName = "New Role Name";

        // Act
        role.SetName(newName);

        // Assert
        role.Name.Should().Be(newName);
    }
}