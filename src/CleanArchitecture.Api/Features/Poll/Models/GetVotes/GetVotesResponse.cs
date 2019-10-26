namespace CleanArchitecture.Api.Features.Poll.Models.GetVotes
{
    using System.Collections.Generic;

    public class GetVotesResponse
    {
        public string Title { get; set; }

        public ICollection<OptionResponse> Options { get; set; }
    }
}
