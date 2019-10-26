namespace CleanArchitecture.Application.Boundaries.Vote
{
    using System.Threading.Tasks;

    public interface IVoteInputBoundary
    {
        Task HandleAsync(VoteInput input, IVoteOutputBoundary output);
    }
}