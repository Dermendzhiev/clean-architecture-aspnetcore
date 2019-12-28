namespace CleanArchitecture.Application.Boundaries.GetPoll
{
    using System.Threading.Tasks;

    public interface IGetPollInputBoundary
    {
        Task HandleAsync(int input, IGetPollOutputBoundary output);
    }
}