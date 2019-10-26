namespace CleanArchitecture.Application.Boundaries.GetPoll
{
    public interface IGetPollOutputBoundary
    {
        void Success(GetPollOutput output);

        void NotFound(string message);
    }
}
