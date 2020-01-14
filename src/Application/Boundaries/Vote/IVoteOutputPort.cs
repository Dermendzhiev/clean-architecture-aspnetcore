namespace CleanArchitecture.Application.Boundaries.Vote
{
    public interface IVoteOutputPort
    {
        void Success();

        void Error(string message);

        void NotFound(string message);
    }
}