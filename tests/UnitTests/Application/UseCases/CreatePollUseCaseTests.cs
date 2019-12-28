namespace CleanArchitecture.UnitTests.Application.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.CreatePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.UseCases;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Domain.SeedWork;
    using FakeItEasy;
    using Xunit;

    public class CreatePollUseCaseTests
    {
        [Fact]
        public async Task HandleAsync_WithValidPoll_ShouldCallSuccess()
        {
            // Arrange
            IEmailSender emailSenderStub = A.Fake<IEmailSender>();
            IPollRepository pollRepositoryStub = A.Fake<IPollRepository>();

            ICreatePollOutputBoundary outputBoundaryMock = A.Fake<ICreatePollOutputBoundary>();

            var useCase = new CreatePollUseCase(null, emailSenderStub, pollRepositoryStub);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, outputBoundaryMock);

            // Assert
            A.CallTo(() => outputBoundaryMock.Success(
                A<CreatePollOutput>.That.Matches(
                    p => p.Id == 0)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WithValidPoll_ShouldCreateNewPoll()
        {
            // Arrange
            IEmailSender emailSenderStub = A.Fake<IEmailSender>();
            ICreatePollOutputBoundary outputBoundaryStub = A.Fake<ICreatePollOutputBoundary>();

            IPollRepository pollRepositoryMock = A.Fake<IPollRepository>();

            var useCase = new CreatePollUseCase(null, emailSenderStub, pollRepositoryMock);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, outputBoundaryStub);

            // Assert
            A.CallTo(() => pollRepositoryMock.CreateAsync(
                A<Poll>.That.Matches(
                    p => p.Title == input.Title
                    && p.Note == input.Note
                    && p.SingleOptionLimitation == input.SingleOptionLimitation
                    && p.DueDate == input.DueDate
                    && p.Options.Count == input.Options.Count)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WithValidPoll_ShouldSendEmail()
        {
            // Arrange
            IPollRepository pollRepositoryStub = A.Fake<IPollRepository>();
            ICreatePollOutputBoundary outputBoundaryStub = A.Fake<ICreatePollOutputBoundary>();

            IEmailSender emailSenderMock = A.Fake<IEmailSender>();

            var useCase = new CreatePollUseCase(null, emailSenderMock, pollRepositoryStub);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, outputBoundaryStub);

            // Assert
            A.CallTo(() => emailSenderMock.SendAsync(
                A<string>.That.Contains("SUBJECT"),
                A<string>.That.Contains("PLAIN_TEXT_CONTENT"),
                //A<List<string>>.Ignored))
                A<List<string>>.That.IsSameSequenceAs(input.ParticipantEmailAddresses)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidPoll_ShouldLogInformation()
        {
            // Arrange
            const string expectedLogMessage = "{@excepton} occured when trying to create a poll with {@input}";
            ICreatePollOutputBoundary outputBoundaryStub = A.Fake<ICreatePollOutputBoundary>();

            ILoggerService<CreatePollUseCase> loggerServiceMock = A.Fake<ILoggerService<CreatePollUseCase>>();
            
            var useCase = new CreatePollUseCase(loggerServiceMock, null, null);
            CreatePollInput input = GetInvalidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, outputBoundaryStub);

            // Assert
            A.CallTo(
                () => loggerServiceMock.LogInformation(
                    A<string>.That.Contains(expectedLogMessage),
                    A<DomainException>._,
                    A<CreatePollInput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidPoll_ShouldCallError()
        {
            // Arrange
            ILoggerService<CreatePollUseCase> loggerServiceStub = A.Fake<ILoggerService<CreatePollUseCase>>();

            ICreatePollOutputBoundary outputBoundaryMock = A.Fake<ICreatePollOutputBoundary>();

            var useCase = new CreatePollUseCase(loggerServiceStub, null, null);
            CreatePollInput input = GetInvalidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, outputBoundaryMock);

            // Assert
            A.CallTo(() => outputBoundaryMock.Error(
                A<string>.That.IsNotNull()))
                .MustHaveHappenedOnceExactly();
        }

        private static CreatePollInput GetValidCreatePollInput()
        {
            return new CreatePollInput("test title", "test note", false, DateTime.Now, new List<string>
            {
                "Option 1",
                "Option 2"
            }, new List<string>
            {
                "test@email.com"
            });
        }

        private static CreatePollInput GetInvalidCreatePollInput()
        {
            return new CreatePollInput(null, "test note", false, DateTime.Now, new List<string>
            {
                "Option 1",
                "Option 2"
            }, new List<string>
            {
                "test@email.com"
            });
        }
    }
}
