namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    public interface ICreatePollOutputPort
    {
        void Success(CreatePollOutput output);

        void Error(string message);
    }
}
