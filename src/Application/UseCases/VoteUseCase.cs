namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.Boundaries.Vote;
    using CleanArchitecture.Domain.SeedWork;

    public class VoteUseCase : IVoteInputPort
    {
        private readonly ILoggerService<VoteUseCase> loggerService;
        private readonly IPollGateway pollGateway;
        private readonly IDateTime dateTime;

        public VoteUseCase(ILoggerService<VoteUseCase> loggerService, IPollGateway pollGateway, IDateTime dateTime)
        {
            this.loggerService = loggerService;
            this.pollGateway = pollGateway;
            this.dateTime = dateTime;
        }

        public async Task HandleAsync(VoteInput input, IVoteOutputPort output)
        {
            try
            {
                Poll poll = await this.pollGateway.GetAsync(input.Id);
                if (poll is null)
                {
                    output.NotFound("Poll not found!");
                    this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", input.Id);
                    return;
                }

                poll.Vote(input.ParticipantEmailAddress, input.Options, this.dateTime.UtcNow);

                await this.pollGateway.UpdateAsync(poll);

                output.Success();
            }
            catch (DomainException ex)
            {
                this.loggerService.LogInformation("{@excepton} occured when trying to vote with {@input}", ex, input);
                output.Error(ex.Message);
            }
        }
    }
}
