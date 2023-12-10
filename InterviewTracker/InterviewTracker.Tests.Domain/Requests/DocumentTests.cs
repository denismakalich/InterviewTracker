using System.Globalization;
using AutoFixture;
using Domain.Entities;
using Domain.Entities.Requests;
using FluentAssertions;
using InterviewTracker.Tests.Domain.Extention;
using Xunit;

namespace InterviewTracker.Tests.Domain.Requests
{
    public class DocumentTests
    {
        private readonly IFixture _fixture;

        public DocumentTests()
        {
            _fixture = new Fixture();
            _fixture.ExecuteAllCustomizations();
        }

        [Fact]
        public void Constructor_ValidArguments_ShouldCreateDocument()
        {
            // Arrange
            string name = _fixture.Create<string>();
            Email email = new Email("john@example.com");
            DateTime age = _fixture.Create<DateTime>();
            int experience = 3;

            // Act
            var document = new Document(name, email, age, experience);

            // Assert
            document.Should().NotBeNull();
            document.Name.Should().Be(name);
            document.Email.Should().Be(email);
            document.Age.Should().Be(age);
            document.Experience.Should().Be(experience);
        }
        
        [Fact]
        public void Create_ValidParameters_ShouldCreateDocument()
        {
            // Arrange
            string name = _fixture.Create<string>();
            Email email = new Email("john@example.com");
            DateTime age = _fixture.Create<DateTime>();
            int experience = 3;

            // Act
            var document = Document.Create(name, email, age, experience);

            // Assert
            document.Should().NotBeNull();
            document.Name.Should().Be(name);
            document.Email.Should().Be(email);
            document.Age.Should().Be(age);
            document.Experience.Should().Be(experience);
        }

        [Fact]
        public void Constructor_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            string name = "John Doe";
            Email email = null!;
            DateTime age = _fixture.Create<DateTime>();
            int experience = 3;

            // Act & Assert
            Action act = () => new Document(name, email, age, experience);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'email')");
        }

        [Fact]
        public void Create_InvalidName_ShouldThrowException()
        {
            string name = "";
            // Act & Assert
            Action act = () => Document.Create(name, new Email("john@example.com"), _fixture.Create<DateTime>(), 3);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("The name cannot be whitespace or null.*name*", because: name);
        }

        [Fact]
        public void Constructor_InvalidAge_ShouldThrowException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "John Doe";
            Email email = new Email("john@example.com");
            int experience = 3;
            DateTime invalidAge = new DateTime(2023, 12, 11); 

            // Act & Assert
            Action act = () => new Document(name, email, invalidAge, experience);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Age is not filled in or filled in incorrectly*");
        }

        [Fact]
        public void Constructor_InvalidExperience_ShouldThrowException()
        {
            // Arrange
            string name = "John Doe";
            Email email = new Email("john@example.com");
            DateTime age = new DateTime(1980, 12, 11); 
            int invalidExperience = -1;

            // Act & Assert
            Action act = () => new Document(name, email, age, invalidExperience);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("The experience is not filled out or filled out incorrectly*");
        }
    }
}