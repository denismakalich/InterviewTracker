using AutoFixture;
using Domain.Entities.Requests.Events;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests.Events;

public class RequestRejectEventTests
{
    private readonly IFixture _fixture;

    public RequestRejectEventTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_RequestRejectEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = new RequestRejectEvent(id, data, requestId);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }

    [Fact]
    public void Create_RequestRejectEvent_WithEmptyId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        Action action = () => new RequestRejectEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void Create_RequestRejectEvent_WithEmptyRequestId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.Empty;
        var data = "Approval data";

        // Act
        Action action = () => new RequestRejectEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("RequestId cannot be empty (Parameter 'requestId')");
    }

    [Fact]
    public void Create_RequestRejectEvent_WithEmptyData_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = string.Empty;

        // Act
        Action action = () => new RequestRejectEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void Create_RequestRejectEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = RequestRejectEvent.Create(data, requestId);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }
}