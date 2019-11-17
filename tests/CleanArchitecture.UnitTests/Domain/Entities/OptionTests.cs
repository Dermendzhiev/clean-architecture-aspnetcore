namespace CleanArchitecture.UnitTests.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Domain.SeedWork;
    using CleanArchitecture.Domain.SharedKernel;
    using FluentAssertions;
    using Xunit;

    public class OptionTests
    {
        [Fact]
        public void Initialize_WithValidData_ShouldCreateNewOption()
        {
            // arrange
            const string text = "fake text";

            // act
            var option = new Option(text);

            // assert
            option.Text.Should().Be(text);
            option.Votes.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Initialize_WithInvalidText_ShouldThrowDomainException(string text)
        {
            // arrange
            const string expectedExceptionMessage = "Text cannot be null or empty";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => new Option(text));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetText_WhenValidText_ShouldSetText()
        {
            // arrange
            const string text = "new text";
            Option option = this.GetOption();

            // act
            option.SetText(text);

            // assert
            option.Text.Should().Be(text);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetText_WhenNullOrEmptyText_ShouldThrowDomainException(string text)
        {
            // arrange
            const string expectedExceptionMessage = "Text cannot be null or empty";
            Option option = this.GetOption();

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => option.SetText(text));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void AddVote_WithValidData_ShouldAddNewVote()
        {
            // arrange
            const string participantEmailAddress = "test@gmail.com";
            Option option = this.GetOption();

            // act
            option.AddVote(participantEmailAddress, DateTime.Now);

            // assert
            option.Votes.Count.Should().Be(1);
        }

        [Fact]
        public void AddVote_WithAlreadyExistingParticipant_ShouldThrowDomainException()
        {
            // arrange
            const string participantEmailAddress = "test@gmail.com";
            Option option = this.GetOption();
            option.AddVote(participantEmailAddress, DateTime.Now);
            const string expectedExceptionMessage = "You have already voted for this poll";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => option.AddVote(participantEmailAddress, DateTime.Now));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        private Option GetOption()
        {
            const string text = "fake text";

            return new Option(text);
        }
    }
}
