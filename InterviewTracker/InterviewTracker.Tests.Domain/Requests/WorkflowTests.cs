using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;
using Domain.Entities.Templates;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests;

public class WorkflowTests
{
    private readonly IFixture _fixture;

    public WorkflowTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_ValidArguments_ShouldCreateWorkflowInstance()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = _fixture.Create<string>();
        Guid workflowTemplateId = Guid.NewGuid();
        List<WorkflowStep> steps = new List<WorkflowStep>();

        // Act
        var workflow = new Workflow(id, name, workflowTemplateId, steps);

        // Assert
        workflow.Should().NotBeNull();
        workflow.Id.Should().Be(id);
        workflow.Name.Should().Be(name);
        workflow.WorkflowTemplateId.Should().Be(workflowTemplateId);
        workflow.ReadSteps.Should().BeEquivalentTo(steps);
    }

    [Fact]
    public void Constructor_EmptyId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid id = Guid.Empty;
        string name = _fixture.Create<string>();
        Guid workflowTemplateId = Guid.NewGuid();
        List<WorkflowStep> steps = new List<WorkflowStep>();

        // Act & Assert
        Action act = () => new Workflow(id, name, workflowTemplateId, steps);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("id cannot be empty or null.*id*", because: "id");
    }

    [Fact]
    public void Constructor_EmptyWorkflowTemplateId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = "Sample Workflow";
        Guid workflowTemplateId = Guid.Empty;
        List<WorkflowStep> steps = new List<WorkflowStep>();

        // Act & Assert
        Action act = () => new Workflow(id, name, workflowTemplateId, steps);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("workflowTemplateId cannot be empty or null.*workflowTemplateId*",
                because: "workflowTemplateId");
    }

    [Fact]
    public void Constructor_NullSteps_ShouldThrowArgumentNullException()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = "Sample Workflow";
        Guid workflowTemplateId = Guid.NewGuid();
        List<WorkflowStep> steps = null;

        // Act & Assert
        Action act = () => new Workflow(id, name, workflowTemplateId, steps);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'steps')");
    }

    [Fact]
    public void SetName_ValidName_ShouldSetName()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "OldName", Guid.NewGuid(), new List<WorkflowStep>());
        string newName = "NewName";

        // Act
        workflow.SetName(newName);

        // Assert
        workflow.Name.Should().Be(newName);
    }

    [Fact]
    public void SetName_NullName_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "OldName", Guid.NewGuid(), new List<WorkflowStep>());
        string newName = null;

        // Act & Assert
        Action act = () => workflow.SetName(newName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The name cannot be empty or null*name*", because: "name");
    }

    [Fact]
    public void SetName_EmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "OldName", Guid.NewGuid(), new List<WorkflowStep>());
        string newName = "";

        // Act & Assert
        Action act = () => workflow.SetName(newName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The name cannot be empty or null*name*", because: "name");
    }

    [Fact]
    public void Create_ValidArguments_ShouldCreateWorkflowInstance()
    {
        // Arrange
        var workflowTemplate = new WorkflowTemplate(Guid.NewGuid(), "Template", new List<WorkflowStepTemplate>());
        string name = "Sample Workflow";

        // Act
        var workflow = Workflow.Create(name, workflowTemplate);

        // Assert
        workflow.Should().NotBeNull();
        workflow.Name.Should().Be(name);
        workflow.WorkflowTemplateId.Should().Be(workflowTemplate.Id);
        workflow.ReadSteps.Should().BeEmpty();
    }

    [Fact]
    public void Restart_ShouldResetStepsToEmptyList()
    {
        // Arrange
        var workflow = _fixture.Create<Workflow>();

        // Act
        workflow.Restart();

        // Assert
        workflow.ReadSteps.Should().BeEmpty();
    }

    [Fact]
    public void IsApprove_WithLastStepApprove_ShouldReturnTrue()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var workflow = new Workflow(Guid.NewGuid(), _fixture.Create<string>(), Guid.NewGuid(), new List<WorkflowStep>());
        
        // Act
        workflow.AddStep("Step1", Status.Approve, null, Guid.NewGuid(), "Comment1");

        // Assert
        workflow.IsApprove().Should().BeTrue();
    }

    [Fact]
    public void IsApprove_WithRejectSteps_ShouldReturnFalse()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        workflow.AddStep("Step1", Status.Reject, user, Guid.NewGuid(), "Comment1");

        // Act & Assert
        workflow.IsApprove().Should().BeFalse();
    }

    [Fact]
    public void IsReject_WithNoRejectSteps_ShouldReturnFalse()
    {
        // Arrange
        var workflow = _fixture.Create<Workflow>();

        // Act & Assert
        workflow.IsReject().Should().BeFalse();
    }

    [Fact]
    public void AddStep_WithUserAndRoleId_ShouldAddStep()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "SampleWorkflow", Guid.NewGuid(), new List<WorkflowStep>());
        var user = _fixture.Create<User>();

        // Act
        workflow.AddStep("Step1", Status.Pending, user, Guid.Empty, "Comment1");

        // Assert
        workflow.ReadSteps.Should().ContainSingle();
        var addedStep = workflow.ReadSteps[0];
        addedStep.Name.Should().Be("Step1");
        addedStep.Status.Should().Be(Status.Pending);
        addedStep.UserId.Should().Be(user.Id);
        addedStep.Comment.Should().Be("Comment1");
    }

    [Fact]
    public void AddStep_WithUserAndEmptyRoleId_ShouldAddStep()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "SampleWorkflow", Guid.NewGuid(), new List<WorkflowStep>());
        var user = _fixture.Create<User>();

        // Act
        workflow.AddStep("Step1", Status.Pending, user, Guid.Empty, "Comment1");

        // Assert
        workflow.ReadSteps.Should().ContainSingle();
        var addedStep = workflow.ReadSteps[0];
        addedStep.Name.Should().Be("Step1");
        addedStep.Status.Should().Be(Status.Pending);
        addedStep.UserId.Should().Be(user.Id);
        addedStep.RoleId.Should().BeEmpty();
        addedStep.Comment.Should().Be("Comment1");
    }

    [Fact]
    public void AddStep_WithEmptyUserAndRoleId_ShouldAddStep()
    {
        // Arrange
        var workflow = new Workflow(Guid.NewGuid(), "SampleWorkflow", Guid.NewGuid(), new List<WorkflowStep>());
        var roleId = Guid.NewGuid();

        // Act
        workflow.AddStep("Step1", Status.Pending, null, roleId, "Comment1");

        // Assert
        workflow.ReadSteps.Should().ContainSingle();
        var addedStep = workflow.ReadSteps[0];
        addedStep.Name.Should().Be("Step1");
        addedStep.Status.Should().Be(Status.Pending);
        addedStep.RoleId.Should().Be(roleId);
        addedStep.Comment.Should().Be("Comment1");
    }
}