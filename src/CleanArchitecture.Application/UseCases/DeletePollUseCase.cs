namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Domain.Entities;

    public class DeletePollUseCase : IDeletePollIntputBoundary
    {
        private readonly ILoggerService<DeletePollUseCase> loggerService;
        private readonly IPollRepository pollRepository;

        public DeletePollUseCase(ILoggerService<DeletePollUseCase> loggerService, IPollRepository pollRepository)
        {
            this.loggerService = loggerService;
            this.pollRepository = pollRepository;
        }

        public async Task HandleAsync(int id, IDeletePollOutputBoundary output)
        {
            Poll poll = await this.pollRepository.GetAsync(id);
            if (poll is null)
            {
                output.NotFound("Poll not found!");
                this.loggerService.LogInformation("Cannot retrieve a poll with {@id}", id);
                return;
            }

            await this.pollRepository.DeleteAsync(poll);
            output.Success();
        }
    }
}
