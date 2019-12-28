namespace CleanArchitecture.UnitTests.Domain.Entities
{
    using System;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Domain.SeedWork;
    using FluentAssertions;
    using Xunit;

    public class OptionTests
    {
        [Fact]
        public void Initialize_WithValidData_ShouldCreateNewOption()
        {
            // Arrange
            const string text = "fake text";

            // Act
            var option = new Option(text);

            // Assert
            option.Text.Should().Be(text);
            option.Votes.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Initialize_WithInvalidText_ShouldThrowDomainException(string text)
        {
            // Arrange
            const string expectedExceptionMessage = "Text cannot be null or empty";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => new Option(text));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetText_WhenValidText_ShouldSetText()
        {
            // Arrange
            const string text = "new text";
            Option option = this.GetOption();

            // Act
            option.SetText(text);

            // Assert
            option.Text.Should().Be(text);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetText_WhenNullOrEmptyText_ShouldThrowDomainException(string text)
        {
            // Arrange
            const string expectedExceptionMessage = "Text cannot be null or empty";
            Option option = this.GetOption();

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => option.SetText(text));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void AddVote_WithValidData_ShouldAddNewVote()
        {
            // Arrange
            const string participantEmailAddress = "test@gmail.com";
            Option option = this.GetOption();

            // Act
            option.AddVote(participantEmailAddress, DateTime.Now);

            // Assert
            option.Votes.Count.Should().Be(1);
        }

        [Fact]
        public void AddVote_WithAlreadyExistingParticipant_ShouldThrowDomainException()
        {
            // Arrange
            const string participantEmailAddress = "test@gmail.com";
            Option option = this.GetOption();
            option.AddVote(participantEmailAddress, DateTime.Now);
            const string expectedExceptionMessage = "You have already voted for this poll";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => option.AddVote(participantEmailAddress, DateTime.Now));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        private Option GetOption()
        {
            const string text = "fake text";

            return new Option(text);
        }
    }
}
