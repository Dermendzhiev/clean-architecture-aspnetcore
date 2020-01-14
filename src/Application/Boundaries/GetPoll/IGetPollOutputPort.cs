namespace CleanArchitecture.Application.Boundaries.GetPoll
{
    public interface IGetPollOutputPort
    {
        void Success(GetPollOutput output);

        void NotFound(string message);
    }
}
