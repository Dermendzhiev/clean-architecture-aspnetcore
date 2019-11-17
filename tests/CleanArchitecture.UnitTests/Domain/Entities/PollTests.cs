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
            // arrange
            const string title = "fake title";
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(1);

            // act
            var poll = new Poll(title, note, singleOptionlimitation, dueDate);

            // assert
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
            // arrange
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(1);
            const string expectedExceptionMessage = "Title cannot be null or empty";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => new Poll(title, note, singleOptionlimitation, dueDate));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Initialize_WhenDueDateInThePast_ShouldThrowDomainException()
        {
            // arrange
            const string title = "fake title";
            const string note = "fake note";
            const bool singleOptionlimitation = true;
            DateTime dueDate = DateTime.Now.AddDays(-1);
            const string expectedExceptionMessage = "Cannot set due date in the past";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => new Poll(title, note, singleOptionlimitation, dueDate));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetTitle_WhenValidTitle_ShouldSetTitle()
        {
            // arrange
            Poll poll = this.GetPoll();
            const string title = "new title";

            // act
            poll.SetTitle(title);

            // assert
            poll.Title.Should().Be(title);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetTitle_WhenNullOrEmptyTitle_ShouldThrowDomainException(string title)
        {
            // arrange
            Poll poll = this.GetPoll();
            const string expectedExceptionMessage = "Title cannot be null or empty";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.SetTitle(title));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void SetDueDate_WhenValidDueDate_ShouldSetDueDate()
        {
            // arrange
            Poll poll = this.GetPoll();
            DateTime dueDate = DateTime.Now.AddDays(1);

            // act
            poll.SetDueDate(dueDate);

            // assert
            poll.DueDate.Should().Be(dueDate);
        }

        [Fact]
        public void SetDueDate_WhenDueDateInThePast_ShouldThrowDomainException()
        {
            // arrange
            Poll poll = this.GetPoll();
            string expectedExceptionMessage = "Cannot set due date in the past";
            DateTime invalidDueDate = DateTime.Now.AddDays(-1);
            SystemTime.Set(DateTime.Now.AddDays(1));

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.SetDueDate(invalidDueDate));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void AddOption_WhenValidOption_MustAddOption()
        {
            // arrange
            Poll poll = this.GetPoll();
            string optionText = "fake text";
            int expectedResult = 1;

            // act
            poll.AddOption(optionText);

            // assert
            poll.Options.Count.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddOption_WhenNullOrEmptyTitle_ShouldThrowDomainException(string option)
        {
            // arrange
            Poll poll = this.GetPoll();
            const string expectedExceptionMessage = "Option cannot be null or empty";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.AddOption(option));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WithValidData_MustVoteForOption()
        {
            // arrange
            Poll poll = this.GetPoll();
            poll.AddOption("test option text");
            var optionIds = new List<int> { poll.Options.First().Id };

            // act
            poll.Vote(null, optionIds, default);

            // assert
            poll.Options.First().Votes.Count.Should().Be(1);
        }

        [Fact]
        public void Vote_ForMultipleOptionsWhenLimitated_ShouldThrowDomainException()
        {
            // arrange
            Poll poll = this.GetPoll(true);
            var optionIds = new List<int> { 1, 2 };
            const string expectedExceptionMessage = "Cannot vote for more than 1 options";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WhenPollHasExpired_ShouldThrowDomainException()
        {
            // arrange
            Poll poll = this.GetPoll();
            var optionIds = new List<int> { 1 };
            SystemTime.Set(DateTime.Now.AddDays(1));
            const string expectedExceptionMessage = "Poll has expired";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // assert
            actualException.Message.Should().Contain(expectedExceptionMessage);
        }

        [Fact]
        public void Vote_WhenOptionIsInvalid_ShouldThrowDomainException()
        {
            // arrange
            Poll poll = this.GetPoll();
            var optionIds = new List<int> { 1 };
            const string expectedExceptionMessage = "Option with id 1 doesn't exist";

            // act
            DomainException actualException = Assert.Throws<DomainException>(() => poll.Vote(null, optionIds, default));

            // assert
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
