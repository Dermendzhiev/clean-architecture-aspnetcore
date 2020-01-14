namespace CleanArchitecture.Application.Boundaries.UpdatePoll
{
    public interface IUpdatePollOutputPort
    {
        void Success();

        void Error(string message);

        void NotFound(string message);
    }
}