namespace CleanArchitecture.Application.Boundaries.DeletePoll
{
    public interface IDeletePollOutputPort
    {
        void Success();

        void NotFound(string message);
    }
}