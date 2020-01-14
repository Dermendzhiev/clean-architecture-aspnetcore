namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.Entities;

    public class GetPollUseCase : IGetPollInputPort
    {
        private readonly ILoggerService<GetPollUseCase> loggerService;
        private readonly IPollGateway pollGateway;

        public GetPollUseCase(ILoggerService<GetPollUseCase> loggerService, IPollGateway pollGateway)
        {
            this.loggerService = loggerService;
            this.pollGateway = pollGateway;
        }

        public async Task HandleAsync(int id, IGetPollOutputPort output)
        {
            Poll poll = await this.pollGateway.GetAsync(id);
            if (poll is null)
            {
                output.NotFound("Poll not found");
                this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", id);
                return;
            }

            output.Success(new GetPollOutput(poll.Id, poll.Title, poll.Note, poll.DueDate, poll.SingleOptionLimitation, poll.Options));
        }
    }
}
