namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    public interface IGetVotesOutputBoundary
    {
        void Success(GetVotesOutput output);

        void Error(string message);

        void NotFound(string message);
    }
}
