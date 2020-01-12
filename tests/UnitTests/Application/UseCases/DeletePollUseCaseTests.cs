namespace CleanArchitecture.UnitTests.Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.UseCases;
    using CleanArchitecture.Domain.Entities;
    using FakeItEasy;
    using Xunit;

    public class DeletePollUseCaseTests
    {
        [Fact]
        public async Task HandleAsync_WhenPollExists_ShouldCallSuccess()
        {
            // Arrange
            const int pollId = 1;
            Poll poll = this.GetFakePoll();

            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns(poll);

            IDeletePollOutputBoundary outputBoundaryMock = A.Fake<IDeletePollOutputBoundary>();

            var useCase = new DeletePollUseCase(null, pollGatewayStub);

            // Act
            await useCase.HandleAsync(pollId, outputBoundaryMock);

            // Assert
            A.CallTo(() => outputBoundaryMock.Success()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WhenPollExists_ShouldDeleteIt()
        {
            // Arrange
            const int pollId = 1;
            Poll poll = this.GetFakePoll();

            IPollGateway pollGatewayMock = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayMock.GetAsync(pollId))
                .Returns(poll);

            IDeletePollOutputBoundary outputBoundaryStub = A.Fake<IDeletePollOutputBoundary>();

            var useCase = new DeletePollUseCase(null, pollGatewayMock);

            // Act
            await useCase.HandleAsync(pollId, outputBoundaryStub);

            // Assert
            A.CallTo(() => pollGatewayMock.DeleteAsync(poll)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WhenPollDoesNotExist_ShouldCallNotFound()
        {
            // Arrange
            const int pollId = 1;
            const string expectedResultMessage = "Poll not found";

            ILoggerService<DeletePollUseCase> loggerServiceStub = A.Fake<ILoggerService<DeletePollUseCase>>();
            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns((Poll)null);

            IDeletePollOutputBoundary outputBoundaryMock = A.Fake<IDeletePollOutputBoundary>();

            var useCase = new DeletePollUseCase(loggerServiceStub, pollGatewayStub);

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

            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            A.CallTo(() => pollGatewayStub.GetAsync(pollId))
                .Returns((Poll)null);

            IDeletePollOutputBoundary outputBoundaryStub = A.Fake<IDeletePollOutputBoundary>();
            ILoggerService<DeletePollUseCase> loggerServiceMock = A.Fake<ILoggerService<DeletePollUseCase>>();

            var useCase = new DeletePollUseCase(loggerServiceMock, pollGatewayStub);

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
