namespace CleanArchitecture.Application.UseCases
{
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Application.Boundaries.UpdatePoll;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Domain.SeedWork;
    using CleanArchitecture.Application.Interfaces.Infrastructure;

    public class UpdatePollUseCase : IUpdatePollIntputPort
    {
        private readonly ILoggerService<UpdatePollUseCase> loggerService;
        private readonly IPollGateway pollGateway;

        public UpdatePollUseCase(ILoggerService<UpdatePollUseCase> loggerService, IPollGateway pollGateway)
        {
            this.loggerService = loggerService;
            this.pollGateway = pollGateway;
        }

        public async Task HandleAsync(UpdatePollInput input, IUpdatePollOutputPort output)
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

                poll.SetTitle(input.Title);
                poll.SetDueDate(input.DueDate);

                await this.pollGateway.UpdateAsync(poll);

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
