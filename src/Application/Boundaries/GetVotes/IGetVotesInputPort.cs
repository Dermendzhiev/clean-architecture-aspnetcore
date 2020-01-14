namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    using System.Threading.Tasks;

    public interface IGetVotesInputPort
    {
        Task HandleAsync(int input, IGetVotesOutputPort output);
    }
}