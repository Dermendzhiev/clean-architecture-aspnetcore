namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    using System.Collections.Generic;

    public class GetVotesOutput
    {
        public string Title { get; set; }

        public ICollection<OptionOutput> Options { get; set; }
    }
}
