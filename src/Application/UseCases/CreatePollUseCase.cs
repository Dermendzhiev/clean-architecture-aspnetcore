namespace CleanArchitecture.Application.UseCases
{
    using CleanArchitecture.Application.Boundaries.CreatePoll;
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.SeedWork;

    public class CreatePollUseCase : ICreatePollInputBoundary
    {
        private readonly ILoggerService<CreatePollUseCase> loggerService;
        private readonly IEmailSender emailSender;
        private readonly IPollRepository pollRepository;

        public CreatePollUseCase(ILoggerService<CreatePollUseCase> loggerService, IEmailSender emailSender, IPollRepository pollRepository)
        {
            this.loggerService = loggerService;
            this.emailSender = emailSender;
            this.pollRepository = pollRepository;
        }

        public async Task HandleAsync(CreatePollInput input, ICreatePollOutputBoundary output)
        {
            try
            {
                var poll = new Poll(input.Title, input.Note, input.SingleOptionLimitation, input.DueDate);

                foreach (string option in input.Options)
                {
                    poll.AddOption(option);
                }

                await this.pollRepository.CreateAsync(poll);
                await this.emailSender.SendAsync("SUBJECT", "PLAIN_TEXT_CONTENT", input.ParticipantEmailAddresses);

                output.Success(new CreatePollOutput(poll.Id));
            }
            catch (DomainException ex)
            {
                this.loggerService.LogInformation("{@excepton} occured when trying to create a poll with {@input}", ex, input);
                output.Error(ex.Message);
            }
        }
    }
}