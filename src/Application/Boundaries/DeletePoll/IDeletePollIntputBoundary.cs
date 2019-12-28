namespace CleanArchitecture.Application.Boundaries.DeletePoll
{
    using System.Threading.Tasks;

    public interface IDeletePollIntputBoundary
    {
        Task HandleAsync(int input, IDeletePollOutputBoundary output);
    }
}