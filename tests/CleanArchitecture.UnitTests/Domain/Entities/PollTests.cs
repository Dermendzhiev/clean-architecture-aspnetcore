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

    public class PollTests : IDisposable
    {
        [Fact]
        public void Initialize_WithValidData_ShouldCreateNewPoll()
        {
            // Arrange
            const string title = "fake title";
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(1);

            // Act
            var poll = new Poll(title, note, singleOptionlimitation, dueDate);

            // Assert
            poll.Title.Should().Be(title);
            poll.Note.Should().Be(note);
            poll.SingleOptionLimitation.Should().Be(singleOptionlimitation);
            poll.DueDate.Should().Be(dueDate);
            poll.Options.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Initialize_WithInvalidTitle_ShouldThrowDomainException(string title)
        {
            // Arrange
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(1);
            const string expectedExceptionMessage = "Title cannot be null or empty";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => new Poll(title, note, singleOptionlimitation, dueDate));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Initialize_WhenDueDateInThePast_ShouldThrowDomainException()
        {
            // Arrange
            const string title = "fake title";
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(-1);
            const string expectedExceptionMessage = "Cannot set due date in the past";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => new Poll(title, note, singleOptionlimitation, dueDate));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetTitle_WhenValidTitle_ShouldSetTitle()
        {
            // Arrange
            Poll poll = this.GetPoll();
            const string title = "new title";

            // Act
            poll.SetTitle(title);

            // Assert
            poll.Title.Should().Be(title);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetTitle_WhenNullOrEmptyTitle_ShouldThrowDomainException(string title)
        {
            // Arrange
            Poll poll = this.GetPoll();
            const string expectedExceptionMessage = "Title cannot be null or empty";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.SetTitle(title));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetDueDate_WhenValidDueDate_ShouldSetDueDate()
        {
            // Arrange
            Poll poll = this.GetPoll();
            DateTime dueDate = DateTime.Now.AddDays(1);

            // Act
            poll.SetDueDate(dueDate);

            // Assert
            poll.DueDate.Should().Be(dueDate);
        }

        [Fact]
        public void SetDueDate_WhenDueDateInThePast_ShouldThrowDomainException()
        {
            // Arrange
            Poll poll = this.GetPoll();
            string expectedExceptionMessage = "Cannot set due date in the past";
            DateTime invalidDueDate = DateTime.Now.AddDays(-1);
            SystemTime.Set(DateTime.Now.AddDays(1));

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.SetDueDate(invalidDueDate));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void AddOption_WhenValidOption_MustAddOption()
        {
            // Arrange
            Poll poll = this.GetPoll();
            string optionText = "fake text";
            int expectedResult = 1;

            // Act
            poll.AddOption(optionText);

            // Assert
            poll.Options.Count.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddOption_WhenNullOrEmptyTitle_ShouldThrowDomainException(string option)
        {
            // Arrange
            Poll poll = this.GetPoll();
            const string expectedExceptionMessage = "Option cannot be null or empty";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.AddOption(option));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WithValidData_MustVoteForOption()
        {
            // Arrange
            Poll poll = this.GetPoll();
            poll.AddOption("test option text");
            var optionIds = new List<int> { poll.Options.First().Id };

            // Act
            poll.Vote(null, optionIds, default);

            // Assert
            poll.Options.First().Votes.Count.Should().Be(1);
        }

        [Fact]
        public void Vote_ForMultipleOptionsWhenLimitated_ShouldThrowDomainException()
        {
            // Arrange
            Poll poll = this.GetPoll(true);
            var optionIds = new List<int> { 1, 2 };
            const string expectedExceptionMessage = "Cannot vote for more than 1 options";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WhenPollHasExpired_ShouldThrowDomainException()
        {
            // Arrange
            Poll poll = this.GetPoll();
            var optionIds = new List<int> { 1 };
            SystemTime.Set(DateTime.Now.AddDays(1));
            const string expectedExceptionMessage = "Poll has expired";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WhenOptionIsInvalid_ShouldThrowDomainException()
        {
            // Arrange
            Poll poll = this.GetPoll();
            var optionIds = new List<int> { 1 };
            const string expectedExceptionMessage = "Option with id 1 doesn't exist";

            // Act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // Assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        private Poll GetPoll(bool? singleOptionLimitation = true, DateTime? dueDate = default)
        {
            const string title = "fake title";
            const string note = "fake note";

            if (dueDate == default)
            {
                dueDate = DateTime.Now.AddDays(1);
            }

            return new Poll(title, note, singleOptionLimitation.Value, dueDate.Value);
        }

        public void Dispose() => SystemTime.Reset();
    }
}
