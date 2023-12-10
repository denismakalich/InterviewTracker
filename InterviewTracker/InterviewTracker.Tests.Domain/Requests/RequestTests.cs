using System.Globalization;
using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;
using Domain.Entities.Requests.Events;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests;

public class RequestTests
{
    private readonly IFixture _fixture;

    public RequestTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_ValidArguments_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = _fixture.Create<User>();
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.1980", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            _fixture.Create<int>()
        );
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());

        // Act
        var request = new Request(id, user, document, workflow);

        // Assert
        request.Id.Should().Be(id);
        request.User.Should().Be(user);
        request.Document.Should().Be(document);
        request.Workflow.Should().Be(workflow);
        request.ReadEvents.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithEmptyId_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var user = _fixture.Create<User>();
        Guid id = Guid.Empty;

        // Act
        Action act = () => new Request(id, user, document, workflow);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("id cannot be empty or null.*id*", because: id.ToString());
    }

    [Fact]
    public void Reject_WithoutPendingStatus_ShouldThrowArgumentExceptionAndNotAddEvent()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(),document, workflow);
        var user = _fixture.Create<User>();

        request.Workflow.AddStep("Step1", Status.Pending, user, Guid.NewGuid(), "Comment1");

        // Act
        Action act = () => request.Reject(user);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }

    [Fact]
    public void Approve_WithoutPendingStatus_ShouldThrowArgumentExceptionAndNotAddEvent()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(),document, workflow);
        var user = _fixture.Create<User>();

        request.Workflow.AddStep("Step1", Status.Pending, user, Guid.NewGuid(), "Comment1");

        // Act
        Action act = () => request.Approve(user);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }

    [Fact]
    public void Reject_WithNoPendingStatus_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(),
            new Document("ValidName", new Email("valid@example.com"), DateTime.Parse("2000-12-05"), 3),
            workflow);
        var user = _fixture.Create<User>();

        // Act
        Action act = () => request.Reject(user);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }

    [Fact]
    public void Restart_ShouldSetAllStepsToPendingStatus()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(), document, workflow);
        var user = _fixture.Create<User>();

        request.Workflow.AddStep("Step1", Status.Approve, user, Guid.NewGuid(), "Comment1");
        request.Workflow.AddStep("Step2", Status.Reject, user, Guid.NewGuid(), "Comment2");

        // Act
        request.Restart();

        // Assert
        var allStepsPending = request.Workflow.ReadSteps.All(step => step.Status == Status.Pending);
        allStepsPending.Should().BeTrue();
    }
    
    [Fact]
    public void Approve_WithMultiplePendingSteps_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(), document, workflow);
        var user = _fixture.Create<User>();

        request.Workflow.AddStep("Step1", Status.Pending, user, Guid.NewGuid(), "Comment1");
        request.Workflow.AddStep("Step2", Status.Pending, user, Guid.NewGuid(), "Comment2");

        // Act
        Action act = () => request.Approve(user);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }
    
    [Fact]
    public void Approve_InvalidUser_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(), document, workflow);
        var validUser = _fixture.Create<User>();
        User invalidUser = null;

        request.Workflow.AddStep("Step1", Status.Pending, validUser, Guid.NewGuid(), "Comment1");

        // Act
        Action act = () => request.Approve(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }
    
    [Fact]
    public void Reject_InvalidUser_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(),
            new List<WorkflowStep>());
        var document = new Document(
            _fixture.Create<string>(),
            _fixture.Create<Email>(),
            DateTime.ParseExact("05.12.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
            3
        );
        var request = new Request(Guid.NewGuid(), _fixture.Create<User>(), document, workflow);
        var validUser = _fixture.Create<User>();
        User invalidUser = null;

        request.Workflow.AddStep("Step1", Status.Pending, validUser, Guid.NewGuid(), "Comment1");

        // Act
        Action act = () => request.Reject(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No stand by statuses found");
    }
}