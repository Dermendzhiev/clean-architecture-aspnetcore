namespace CleanArchitecture.UnitTests.Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.UseCases;
    using CleanArchitecture.Domain.Entities;
    using FakeItEasy;
    using Xunit;

    public class GetPollUseCaseTests
    {
        [Fact]
        public async Task HandleAsync_WhenPollExists_ShouldCallSuccess()
        {
            // Arrange
            const int pollId = 1;

            Poll poll = this.GetFakePoll();
            poll.AddOption("test option");

            var expectedGetPollOutput = new GetPollOutput(poll.Title, poll.Note, poll.DueDate, poll.SingleOptionLimitation, poll.Options);

            IPollRepository pollRepositoryStub = A.Fake<IPollRepository>();
            A.CallTo(() => pollRepositoryStub.GetAsync(pollId))
                .Returns(poll);

            IGetPollOutputBoundary getPollOutputBoundaryMock = A.Fake<IGetPollOutputBoundary>();

            var useCase = new GetPollUseCase(null, pollRepositoryStub);

            // Act
            await useCase.HandleAsync(pollId, getPollOutputBoundaryMock);

            // Assert
            A.CallTo(
                () => getPollOutputBoundaryMock.Success(
                    A<GetPollOutput>.That.Matches(
                        m => m.Title == poll.Title 
                        && m.Note == poll.Note 
                        && m.DueDate == poll.DueDate 
                        && m.SingleOptionLimitation == poll.SingleOptionLimitation)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WhenPollDoesNotExist_ShouldCallNotFound()
        {
            // Arrange
            const int pollId = 1;
            const string expectedResultMessage = "Poll not found";

            ILoggerService<GetPollUseCase> loggerServiceStub = A.Fake<ILoggerService<GetPollUseCase>>();
            IPollRepository pollRepositoryStub = A.Fake<IPollRepository>();
            A.CallTo(() => pollRepositoryStub.GetAsync(pollId))
                .Returns((Poll)null);

            IGetPollOutputBoundary outputBoundaryMock = A.Fake<IGetPollOutputBoundary>();

            var useCase = new GetPollUseCase(loggerServiceStub, pollRepositoryStub);

            // Act
            await useCase.HandleAsync(pollId, outputBoundaryMock);

            // Assert
            A.CallTo(
                () => outputBoundaryMock.NotFound(
                    A<string>.That.Contains(expectedResultMessage)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WhenPollDoesNotExist_ShouldLogInformation()
        {
            // Arrange
            const int pollId = 1;
            const string expectedLogMessage = "Cannot retrieve a poll with {@id}";

            IPollRepository pollRepositoryStub = A.Fake<IPollRepository>();
            A.CallTo(() => pollRepositoryStub.GetAsync(pollId))
                .Returns((Poll)null);

            IGetPollOutputBoundary outputBoundaryStub = A.Fake<IGetPollOutputBoundary>();
            ILoggerService<GetPollUseCase> loggerServiceMock = A.Fake<ILoggerService<GetPollUseCase>>();

            var useCase = new GetPollUseCase(loggerServiceMock, pollRepositoryStub);

            // Act
            await useCase.HandleAsync(pollId, outputBoundaryStub);

            // Assert
            A.CallTo(
                () => loggerServiceMock.LogInformation(
                    A<string>.That.Contains(expectedLogMessage),
                    A<int>.That.IsEqualTo(pollId)))
                .MustHaveHappenedOnceExactly();
        }

        private Poll GetFakePoll(bool? singleOptionLimitation = true, DateTime? dueDate = default)
        {
            const string title = "fake title";
            const string note = "fake note";

            if (dueDate == default)
            {
                dueDate = DateTime.Now.AddDays(1);
            }

            return new Poll(title, note, singleOptionLimitation.Value, dueDate.Value);
        }
    }
}
