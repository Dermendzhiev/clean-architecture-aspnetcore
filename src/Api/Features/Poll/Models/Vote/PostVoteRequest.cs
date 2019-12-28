namespace CleanArchitecture.Api.Features.Poll.Models.Vote
{
    using System.Collections.Generic;

    public class VoteRequest
    {
        public string ParticipantEmailAddress { get; set; }

        public ICollection<int> Options { get; set; }
    }
}
