namespace CleanArchitecture.Application.Boundaries.Vote
{
    using System.Threading.Tasks;

    public interface IVoteInputPort
    {
        Task HandleAsync(VoteInput input, IVoteOutputPort output);
    }
}