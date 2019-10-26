namespace CleanArchitecture.Application.Boundaries.Vote
{
    using System.Collections.Generic;

    public class VoteInput
    {
        public VoteInput(int id, string participantEmailAddress, ICollection<int> options)
        {
            this.Id = id;
            this.ParticipantEmailAddress = participantEmailAddress;
            this.Options = options;
        }

        public int Id { get; set; }

        public string ParticipantEmailAddress { get; set; }

        public ICollection<int> Options { get; set; }
    }
}
