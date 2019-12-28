namespace CleanArchitecture.Application.UseCases
{
    using System.Linq;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.GetVotes;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Domain.SeedWork;

    public class GetVotesUseCase : IGetVotesInputBoundary
    {
        private readonly ILoggerService<GetVotesUseCase> loggerService;
        private readonly IPollRepository pollRepository;

        public GetVotesUseCase(ILoggerService<GetVotesUseCase> loggerService, IPollRepository pollRepository)
        {
            this.loggerService = loggerService;
            this.pollRepository = pollRepository;
        }

        public async Task HandleAsync(int id, IGetVotesOutputBoundary output)
        {
            try
            {
                Poll poll = await this.pollRepository.GetAsync(id);
                if (poll is null)
                {
                    output.NotFound("Poll not found!");
                    this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", id);
                    return;
                }

                var options = poll.Options
                    .Select(option => new OptionOutput
                    {
                        Option = option.Text,
                        ParticipantEmailAddresss = option?.Votes?.Select(vote => vote?.ParticipantEmailAddress).ToList()
                    })
                    .ToList();

                var getVotesOutput = new GetVotesOutput
                {
                    Title = poll.Title,
                    Options = options
                };

                output.Success(getVotesOutput);
            }
            catch (DomainException ex)
            {
                this.loggerService.LogInformation("{@excepton} occured when trying to retrieve votes for a poll with {@id}", ex, id);
                output.Error(ex.Message);
            }
        }
    }
}