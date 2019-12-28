namespace CleanArchitecture.Application.Boundaries.Vote
{
    public interface IVoteOutputBoundary
    {
        void Success();

        void Error(string message);

        void NotFound(string message);
    }
}