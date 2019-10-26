namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    public interface ICreatePollOutputBoundary
    {
        void Success(CreatePollOutput output);

        void Error(string message);
    }
}
