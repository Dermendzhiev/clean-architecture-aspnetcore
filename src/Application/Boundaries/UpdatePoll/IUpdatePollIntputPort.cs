namespace CleanArchitecture.Application.Boundaries.UpdatePoll
{
    using System.Threading.Tasks;

    public interface IUpdatePollIntputPort
    {
        Task HandleAsync(UpdatePollInput input, IUpdatePollOutputPort output);
    }
}