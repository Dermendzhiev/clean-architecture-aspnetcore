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
            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();

            ICreatePollOutputPort OutputPortMock = A.Fake<ICreatePollOutputPort>();

            var useCase = new CreatePollUseCase(null, emailSenderStub, pollGatewayStub);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, OutputPortMock);

            // Assert
            A.CallTo(() => OutputPortMock.Success(
                A<CreatePollOutput>.That.Matches(
                    p => p.Id == 0)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_WithValidPoll_ShouldCreateNewPoll()
        {
            // Arrange
            IEmailSender emailSenderStub = A.Fake<IEmailSender>();
            ICreatePollOutputPort OutputPortStub = A.Fake<ICreatePollOutputPort>();

            IPollGateway pollGatewayMock = A.Fake<IPollGateway>();

            var useCase = new CreatePollUseCase(null, emailSenderStub, pollGatewayMock);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, OutputPortStub);

            // Assert
            A.CallTo(() => pollGatewayMock.CreateAsync(
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
            IPollGateway pollGatewayStub = A.Fake<IPollGateway>();
            ICreatePollOutputPort OutputPortStub = A.Fake<ICreatePollOutputPort>();

            IEmailSender emailSenderMock = A.Fake<IEmailSender>();

            var useCase = new CreatePollUseCase(null, emailSenderMock, pollGatewayStub);
            CreatePollInput input = GetValidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, OutputPortStub);

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
            ICreatePollOutputPort OutputPortStub = A.Fake<ICreatePollOutputPort>();

            ILoggerService<CreatePollUseCase> loggerServiceMock = A.Fake<ILoggerService<CreatePollUseCase>>();
            
            var useCase = new CreatePollUseCase(loggerServiceMock, null, null);
            CreatePollInput input = GetInvalidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, OutputPortStub);

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

            ICreatePollOutputPort OutputPortMock = A.Fake<ICreatePollOutputPort>();

            var useCase = new CreatePollUseCase(loggerServiceStub, null, null);
            CreatePollInput input = GetInvalidCreatePollInput();

            // Act
            await useCase.HandleAsync(input, OutputPortMock);

            // Assert
            A.CallTo(() => OutputPortMock.Error(
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
