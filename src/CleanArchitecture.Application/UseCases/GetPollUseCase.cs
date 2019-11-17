namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.Entities;

    public class GetPollUseCase : IGetPollInputBoundary
    {
        private readonly ILoggerService<GetPollUseCase> loggerService;
        private readonly IPollRepository pollRepository;

        public GetPollUseCase(ILoggerService<GetPollUseCase> loggerService, IPollRepository pollRepository)
        {
            this.loggerService = loggerService;
            this.pollRepository = pollRepository;
        }

        public async Task HandleAsync(int id, IGetPollOutputBoundary output)
        {
            Poll poll = await this.pollRepository.GetAsync(id);
            if (poll is null)
            {
                output.NotFound("Poll not found");
                this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", id);
                return;
            }

            output.Success(new GetPollOutput(poll.Title, poll.Note, poll.DueDate, poll.SingleOptionLimitation, poll.Options));
        }
    }
}
