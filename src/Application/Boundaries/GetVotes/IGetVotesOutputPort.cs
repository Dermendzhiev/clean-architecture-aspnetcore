namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    public interface IGetVotesOutputPort
    {
        void Success(GetVotesOutput output);

        void Error(string message);

        void NotFound(string message);
    }
}
