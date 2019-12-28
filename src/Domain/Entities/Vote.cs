namespace CleanArchitecture.Domain.Entities
{
    using System;
    using CleanArchitecture.Domain.SeedWork;

    public class Vote : Entity
    {
        public Vote(string participantEmailAddress, DateTime votedAt)
        {
            this.ParticipantEmailAddress = participantEmailAddress;
            this.VotedAt = votedAt;
        }

        public string ParticipantEmailAddress { get; private set; }

        public DateTime VotedAt { get; private set; }
    }
}
