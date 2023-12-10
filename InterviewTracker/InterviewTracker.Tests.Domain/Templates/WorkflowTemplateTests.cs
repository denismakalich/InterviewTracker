using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;
using Domain.Entities.Templates;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Templates;

public class WorkflowTemplateTests
{
    private readonly IFixture _fixture;

    public WorkflowTemplateTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Workflow";
        var steps = new List<WorkflowStepTemplate>();

        // Act
        var workflowTemplate =
            new WorkflowTemplate(id, name, steps);

        // Assert
        workflowTemplate.Should().NotBeNull();
        workflowTemplate.Id.Should().Be(id);
        workflowTemplate.Name.Should().Be(name);
        workflowTemplate.ReadSteps.Should().NotBeNull();
        workflowTemplate.ReadSteps.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithEmptyId_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var name = "Test Workflow";
        var steps = new List<WorkflowStepTemplate>();

        // Act
        Action action = () =>
            new WorkflowTemplate(id, name, steps);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("id cannot be empty or null.*id*", because: "id");
    }

    [Fact]
    public void Constructor_WithNullSteps_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Workflow";

        // Act
        Action action = () =>
            new WorkflowTemplate(id, name, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("*steps*");
    }

    [Fact]
    public void Create_WithValidName_ShouldCreateWorkflowTemplate()
    {
        // Arrange
        var name = "Test Workflow";

        // Act
        var workflowTemplate =
            WorkflowTemplate.Create(name);

        // Assert
        workflowTemplate.Should().NotBeNull();
        workflowTemplate.Id.Should().NotBeEmpty();
        workflowTemplate.Name.Should().Be(name);
        workflowTemplate.ReadSteps.Should().NotBeNull();
        workflowTemplate.ReadSteps.Should().BeEmpty();
    }

    [Fact]
    public void SetName_WithValidName_ShouldSetWorkflowTemplateName()
    {
        // Arrange
        var workflowTemplate =
            WorkflowTemplate.Create("InitialName");
        var newName = "UpdatedName";

        // Act
        workflowTemplate.SetName(newName);

        // Assert
        workflowTemplate.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetName_WithNullOrEmptyName_ShouldThrowArgumentException(string name)
    {
        // Arrange
        var workflowTemplate =
            WorkflowTemplate.Create("InitialName");

        // Act
        Action action = () => workflowTemplate.SetName(name);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("The name cannot be empty or null.*name*", because: "name");
    }

    [Fact]
    public void CreateRequest_WithValidParameters_ShouldCreateRequest()
    {
        // Arrange
        var userName = "Test User";
        var userEmail = new Email("test@example.com");
        var roleId = Guid.NewGuid();
        var user = User.Create(userName, userEmail, roleId);
        
        string documentName = _fixture.Create<string>();
        Email documentEmail = new Email("john@example.com");
        DateTime documentAge = _fixture.Create<DateTime>();
        int experience = 3;

        var document = Document.Create(documentName, documentEmail, documentAge, experience);
        var id = Guid.NewGuid();
        var title = "Test Workflow";
        var steps = _fixture.CreateMany<WorkflowStepTemplate>();
        var workflowTemplate =
            new WorkflowTemplate(id, title, steps.ToList());

        // Act
        var request = workflowTemplate.CreateRequest(user, document);

        // Assert
        request.Should().NotBeNull();
        request.User.Should().Be(user);
        request.Document.Should().Be(document);
        request.Workflow.Should().NotBeNull();
        request.Workflow.Name.Should().Be("Test Workflow");
        request.Workflow.WorkflowTemplateId.Should().Be(workflowTemplate.Id);
        request.Workflow.ReadSteps.Should().HaveCount(workflowTemplate.ReadSteps.Count);
    }
}