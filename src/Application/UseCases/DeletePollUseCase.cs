namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.Entities;

    public class DeletePollUseCase : IDeletePollIntputPort
    {
        private readonly ILoggerService<DeletePollUseCase> loggerService;
        private readonly IPollGateway pollGateway;

        public DeletePollUseCase(ILoggerService<DeletePollUseCase> loggerService, IPollGateway pollGateway)
        {
            this.loggerService = loggerService;
            this.pollGateway = pollGateway;
        }

        public async Task HandleAsync(int id, IDeletePollOutputPort output)
        {
            Poll poll = await this.pollGateway.GetAsync(id);
            if (poll is null)
            {
                output.NotFound("Poll not found");
                this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", id);
                return;
            }

            await this.pollGateway.DeleteAsync(poll);
            output.Success();
        }
    }
}
