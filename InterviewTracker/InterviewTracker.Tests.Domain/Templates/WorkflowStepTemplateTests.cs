using AutoFixture;
using Domain.Entities.Templates;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Templates
{
    public class WorkflowStepTemplateTests
    {
        private readonly IFixture _fixture;

        public WorkflowStepTemplateTests()
        {
            _fixture = new Fixture();
            _fixture.ExecuteAllCustomizations();
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();
            var roleId = _fixture.Create<Guid>();

            // Act
            var step = new WorkflowStepTemplate(name, userId, roleId);

            // Assert
            step.Should().NotBeNull();
            step.Name.Should().Be(name);
            step.UserId.Should().Be(userId);
            step.RoleId.Should().Be(roleId);
            step.Order.Should().Be(0); // Assuming the default order is 0
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(invalidName, _fixture.Create<Guid>(), _fixture.Create<Guid>());
            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be empty or null or white space.*name*", because: invalidName);
        }

        [Fact]
        public void Constructor_WithEmptyUserId_ShouldThrowArgumentException()
        {
            Guid id = Guid.Empty;
            
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(_fixture.Create<string>(), id, _fixture.Create<Guid>());
            action.Should().Throw<ArgumentException>().WithMessage("Id cannot be empty or null.*userId*", because: id.ToString());
        }

        [Fact]
        public void Constructor_WithEmptyRoleId_ShouldThrowArgumentException()
        {
            Guid id = Guid.Empty;
            
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(_fixture.Create<string>(), _fixture.Create<Guid>(), id);
            action.Should().Throw<ArgumentException>().WithMessage("Id cannot be empty or null.*roleId*", because: id.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ShouldThrowArgumentExceptionWithCorrectParamName(string invalidName)
        {
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(invalidName, _fixture.Create<Guid>(), _fixture.Create<Guid>());
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == "name");
        }

        [Fact]
        public void Constructor_WithEmptyUserId_ShouldThrowArgumentExceptionWithCorrectParamName()
        {
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(_fixture.Create<string>(), Guid.Empty, _fixture.Create<Guid>());
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == "userId");
        }

        [Fact]
        public void Constructor_WithEmptyRoleId_ShouldThrowArgumentExceptionWithCorrectParamName()
        {
            // Act & Assert
            Action action = () => new WorkflowStepTemplate(_fixture.Create<string>(), _fixture.Create<Guid>(), Guid.Empty);
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == "roleId");
        }

        [Fact]
        public void Create_WithValidArguments_ShouldCreateInstance()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var userId = _fixture.Create<Guid>();
            var roleId = _fixture.Create<Guid>();

            // Act
            var step = WorkflowStepTemplate.Create(name, userId, roleId);

            // Assert
            step.Should().NotBeNull();
            step.Name.Should().Be(name);
            step.UserId.Should().Be(userId);
            step.RoleId.Should().Be(roleId);
            step.Order.Should().Be(0); // Assuming the default order is 0
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Act & Assert
            Action action = () =>
                WorkflowStepTemplate.Create(invalidName, _fixture.Create<Guid>(), _fixture.Create<Guid>());
            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be empty or null or white space.*name*", because: invalidName);
        }
    }
}