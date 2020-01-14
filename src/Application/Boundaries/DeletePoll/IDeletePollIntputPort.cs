namespace CleanArchitecture.Application.Boundaries.DeletePoll
{
    using System.Threading.Tasks;

    public interface IDeletePollIntputPort
    {
        Task HandleAsync(int input, IDeletePollOutputPort output);
    }
}