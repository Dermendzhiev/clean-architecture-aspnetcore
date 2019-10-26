namespace CleanArchitecture.Application.Boundaries.UpdatePoll
{
    using System.Threading.Tasks;

    public interface IUpdatePollIntputBoundary
    {
        Task HandleAsync(UpdatePollInput input, IUpdatePollOutputBoundary output);
    }
}