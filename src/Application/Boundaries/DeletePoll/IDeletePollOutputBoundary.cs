namespace CleanArchitecture.Application.Boundaries.DeletePoll
{
    public interface IDeletePollOutputBoundary
    {
        void Success();

        void NotFound(string message);
    }
}