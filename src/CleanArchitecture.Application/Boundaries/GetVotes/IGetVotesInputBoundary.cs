namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    using System.Threading.Tasks;

    public interface IGetVotesInputBoundary
    {
        Task HandleAsync(int input, IGetVotesOutputBoundary output);
    }
}