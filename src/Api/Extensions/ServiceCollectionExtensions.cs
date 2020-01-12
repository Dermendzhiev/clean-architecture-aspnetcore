namespace CleanArchitecture.Api.Extensions
{
    using CleanArchitecture.Api.Features.Poll.Presenters;
    using CleanArchitecture.Api.Features.Vote.Presenters;
    using CleanArchitecture.Application.Boundaries.CreatePoll;
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using CleanArchitecture.Application.Boundaries.GetVotes;
    using CleanArchitecture.Application.Boundaries.UpdatePoll;
    using CleanArchitecture.Application.Boundaries.Vote;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    using CleanArchitecture.Application.UseCases;
    using CleanArchitecture.Infrastructure;
    using CleanArchitecture.Infrastructure.Logger;
    using CleanArchitecture.Persistence.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            // Infrastructure:
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));

            // Gateways:
            services.AddScoped<IPollGateway, PollGateway>();

            // Use cases:
            services.AddScoped<ICreatePollInputBoundary, CreatePollUseCase>();
            services.AddScoped<IDeletePollIntputBoundary, DeletePollUseCase>();
            services.AddScoped<IGetPollInputBoundary, GetPollUseCase>();
            services.AddScoped<IGetVotesInputBoundary, GetVotesUseCase>();
            services.AddScoped<IUpdatePollIntputBoundary, UpdatePollUseCase>();
            services.AddScoped<IVoteInputBoundary, VoteUseCase>();

            // Presenters:
            services.AddScoped<CreatePollPresenter>();
            services.AddScoped<DeletePollPresenter>();
            services.AddScoped<GetPollPresenter>();
            services.AddScoped<GetVotesPresenter>();
            services.AddScoped<UpdatePollPresenter>();
            services.AddScoped<GetVotesPresenter>();
            services.AddScoped<VotePresenter>();

            return services;
        }
    }
}
