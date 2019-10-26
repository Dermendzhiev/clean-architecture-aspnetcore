namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.Boundaries.Vote;
    using CleanArchitecture.Domain.SeedWork;

    public class VoteUseCase : IVoteInputBoundary
    {
        private readonly ILoggerService<VoteUseCase> loggerService;
        private readonly IPollRepository pollRepository;
        private readonly IDateTime dateTime;

        public VoteUseCase(ILoggerService<VoteUseCase> loggerService, IPollRepository pollRepository, IDateTime dateTime)
        {
            this.loggerService = loggerService;
            this.pollRepository = pollRepository;
            this.dateTime = dateTime;
        }

        public async Task HandleAsync(VoteInput input, IVoteOutputBoundary output)
        {
            try
            {
                Poll poll = await this.pollRepository.GetAsync(input.Id);
                if (poll is null)
                {
                    output.NotFound("Poll not found!");
                    this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", input.Id);
                    return;
                }

                poll.Vote(input.ParticipantEmailAddress, input.Options, this.dateTime.UtcNow);

                await this.pollRepository.UpdateAsync(poll);

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
