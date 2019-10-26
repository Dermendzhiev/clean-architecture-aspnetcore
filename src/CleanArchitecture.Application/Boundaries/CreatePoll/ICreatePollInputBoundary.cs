namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    using System.Threading.Tasks;

    public interface ICreatePollInputBoundary
    {
        Task HandleAsync(CreatePollInput input, ICreatePollOutputBoundary output);
    }
}
