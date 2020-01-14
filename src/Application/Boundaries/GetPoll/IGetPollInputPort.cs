namespace CleanArchitecture.Application.Boundaries.GetPoll
{
    using System.Threading.Tasks;

    public interface IGetPollInputPort
    {
        Task HandleAsync(int input, IGetPollOutputPort output);
    }
}