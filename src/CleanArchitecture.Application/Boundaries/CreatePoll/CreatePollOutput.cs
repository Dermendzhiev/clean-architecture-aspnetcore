namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    public class CreatePollOutput
    {
        public CreatePollOutput(int id) => this.Id = id;

        public int Id { get; }
    }
}
