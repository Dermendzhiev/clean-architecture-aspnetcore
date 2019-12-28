namespace CleanArchitecture.Application.Boundaries.UpdatePoll
{
    public interface IUpdatePollOutputBoundary
    {
        void Success();

        void Error(string message);

        void NotFound(string message);
    }
}