namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    using System.Threading.Tasks;

    public interface ICreatePollInputPort
    {
        Task HandleAsync(CreatePollInput input, ICreatePollOutputPort output);
    }
}
