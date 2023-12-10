using System.Reflection;
using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests;

public class WorkflowStepTests
{
    private readonly IFixture _fixture;

    public WorkflowStepTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_ValidArguments_ShouldInitializeProperties()
    {
        // Arrange
        var userName = "Test User";
        var userEmail = new Email("test@example.com");
        var userRoleId = Guid.NewGuid();
        var user = User.Create(userName, userEmail, userRoleId);

        var id = Guid.NewGuid();
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        // Act
        var workflowStep = new WorkflowStep(id, name, status, user, roleId, comment);

        // Assert
        workflowStep.Id.Should().Be(id);
        workflowStep.Name.Should().Be(name);
        workflowStep.Order.Should().Be(1);
        workflowStep.Status.Should().Be(status);
        workflowStep.UserId.Should().Be(user.Id);
        workflowStep.RoleId.Should().Be(roleId);
        workflowStep.Comment.Should().Be(comment);
    }

    [Fact]
    public void Constructor_InvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        var userName = "Test User";
        var userEmail = new Email("test@example.com");
        var userRoleId = Guid.NewGuid();
        var user = User.Create(userName, userEmail, userRoleId);

        var id = Guid.Empty;
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        // Act & Assert
        Action act = () => new WorkflowStep(id, name, status, user, roleId, comment);

        act.Should().Throw<ArgumentException>().WithMessage("Id cannot be empty or null.*id*", because: "id");
    }

    [Fact]
    public void CreateByRole_ValidArguments_ShouldCreateWorkflowStepWithCorrectProperties()
    {
        // Arrange
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        // Act
        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Assert
        workflowStep.Name.Should().Be(name);
        workflowStep.Order.Should().Be(1);
        workflowStep.Status.Should().Be(status);
        workflowStep.RoleId.Should().Be(roleId);
        workflowStep.Comment.Should().Be(comment);
    }

    [Fact]
    public void SetName_ValidComment_ShouldSetName()
    {
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        workflowStep.SetName("NewComment");

        // Assert
        workflowStep.Name.Should().Be("NewComment");
    }

    [Fact]
    public void SetName_NullName_ShouldThrowArgumentException()
    {
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        Action act = () => workflowStep.SetName(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The name cannot be empty or null*name*", because: "name");
    }

    [Fact]
    public void SetName_EmptyName_ShouldThrowArgumentException()
    {
        // Arange
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        Action act = () => workflowStep.SetName("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The name cannot be empty or null*name*", because: "name");
    }

    [Fact]
    public void WorkflowStep_UserIdInitializedCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "John Doe";
        var email = new Email("john.doe@example.com");
        var roleId = Guid.NewGuid();

        var user = User.Create(name, email, roleId);

        // Act
        var workflowStep = new WorkflowStep(Guid.NewGuid(), "Name", Status.Pending, user, Guid.NewGuid(), "Comment");

        // Assert
        workflowStep.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void SetComment_ValidComment_ShouldSetTheComment()
    {
        // Arrange
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        workflowStep.SetComment("NewComment");

        // Assert
        workflowStep.Comment.Should().Be("NewComment");
    }

    [Fact]
    public void SetComment_NullComment_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        Action act = () => workflowStep.SetComment(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Comment cannot be empty or null.*comment*", because: "comment");
    }

    [Fact]
    public void SetComment_EmptyComment_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        Action act = () => workflowStep.SetComment("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Comment cannot be empty or null.*comment*", because: "comment");
    }

    [Fact]
    public void SetStatus_WithValidUserAndStatus_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var email = new Email("john.doe@example.com");
        var user = User.Create(_fixture.Create<string>(), email, Guid.NewGuid());

        var name = "StepName";
        var status = Status.Pending;
        var roleId = Guid.NewGuid();
        var comment = "StepComment";

        var workflowStep = WorkflowStep.Create(name, status, roleId, comment);

        // Act
        workflowStep.SetStatus(user, Status.Approve);

        // Assert
        Assert.Equal(user.Id, workflowStep.UserId);
        Assert.Equal(user.RoleId, workflowStep.RoleId);
        Assert.Equal(Status.Approve, workflowStep.Status);
    }
}