namespace CleanArchitecture.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CleanArchitecture.Domain.SeedWork;

    public class Option : Entity
    {
        private readonly List<Vote> votes;

        protected Option() => this.votes = new List<Vote>();

        public Option(string text)
            : this() => this.SetText(text);

        public string Text { get; private set; }

        public IReadOnlyCollection<Vote> Votes => this.votes;

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new DomainException($"Text cannot be null or empty");
            }

            this.Text = text;
        }

        public void AddVote(string participantEmailAddress, DateTime votedAt)
        {
            if (this.votes.Any(v => v.ParticipantEmailAddress.ToLower() == participantEmailAddress.ToLower()))
            {
                throw new DomainException("You have already voted for this poll");
            }

            var vote = new Vote(participantEmailAddress, votedAt);
            this.votes.Add(vote);
        }
    }
}
