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

            var expectedGetPollOutput = new GetPollOutput(poll.Id, poll.Title, poll.Note, poll.DueDate, poll.SingleOptionLimitation, poll.Options);

            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns(poll);

            IGetPollOutputPort getPollOutputPortMock = A.Fake<IGetPollOutputPort>();

            var useCase = new GetPollUseCase(null, pollGatewayStub);

            // Act
            await useCase.HandleAsync(pollId, getPollOutputPortMock);

            // Assert
            A.CallTo(
                () => getPollOutputPortMock.Success(
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
            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns((Poll)null);

            IGetPollOutputPort OutputPortMock = A.Fake<IGetPollOutputPort>();

            var useCase = new GetPollUseCase(loggerServiceStub, pollGatewayStub);

            // Act
            await useCase.HandleAsync(pollId, OutputPortMock);

            // Assert
            A.CallTo(
                () => OutputPortMock.NotFound(
                    A<string>.That.Contains(expectedResultMessage)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WhenPollDoesNotExist_ShouldLogInformation()
        {
            // Arrange
            const int pollId = 1;
            const string expectedLogMessage = "Cannot retrieve a poll with {@id}";

            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns((Poll)null);

            IGetPollOutputPort OutputPortStub = A.Fake<IGetPollOutputPort>();
            ILoggerService<GetPollUseCase> loggerServiceMock = A.Fake<ILoggerService<GetPollUseCase>>();

            var useCase = new GetPollUseCase(loggerServiceMock, pollGatewayStub);

            // Act
            await useCase.HandleAsync(pollId, OutputPortStub);

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
