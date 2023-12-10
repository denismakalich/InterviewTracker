using AutoFixture;
using Domain.Entities.Requests.Events;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests.Events;

public class RequestApproveEventTests
{
    private readonly IFixture _fixture;

    public RequestApproveEventTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_RequestApproveEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = new RequestApproveEvent(id, data, requestId);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }

    [Fact]
    public void Create_RequestApproveEvent_WithEmptyId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        Action action = () => new RequestApproveEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void Create_RequestApproveEvent_WithEmptyRequestId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.Empty;
        var data = "Approval data";

        // Act
        Action action = () => new RequestApproveEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("RequestId cannot be empty (Parameter 'requestId')");
    }

    [Fact]
    public void Create_RequestApproveEvent_WithEmptyData_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = string.Empty;

        // Act
        Action action = () => new RequestApproveEvent(id, data, requestId);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void Create_RequestApproveEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = RequestApproveEvent.Create(data, requestId);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }
}