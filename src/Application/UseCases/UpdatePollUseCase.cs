namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Application.Boundaries.UpdatePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Domain.SeedWork;
    using CleanArchitecture.Application.Interfaces.Infrastructure;

    public class UpdatePollUseCase : IUpdatePollIntputBoundary
    {
        private readonly ILoggerService<UpdatePollUseCase> loggerService;
        private readonly IPollRepository pollRepository;

        public UpdatePollUseCase(ILoggerService<UpdatePollUseCase> loggerService, IPollRepository pollRepository)
        {
            this.loggerService = loggerService;
            this.pollRepository = pollRepository;
        }

        public async Task HandleAsync(UpdatePollInput input, IUpdatePollOutputBoundary output)
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

                poll.SetTitle(input.Title);
                poll.SetDueDate(input.DueDate);

                await this.pollRepository.UpdateAsync(poll);

                output.Success();
            }
            catch (DomainException ex)
            {
                this.loggerService.LogInformation("{@excepton} occured when trying to update a poll with {@input}", ex, input);
                output.Error(ex.Message);
            }
        }
    }
}
