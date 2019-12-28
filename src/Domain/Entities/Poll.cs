namespace CleanArchitecture.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CleanArchitecture.Domain.SeedWork;
    using CleanArchitecture.Domain.SharedKernel;

    public class Poll : Entity
    {
        private readonly List<Option> options;

        protected Poll() => this.options = new List<Option>();

        public Poll(string title, string note, bool singleOptionLimitation, DateTime dueDate)
            : this()
        {
            this.SetTitle(title);
            this.Note = note;
            this.SingleOptionLimitation = singleOptionLimitation;
            this.SetDueDate(dueDate);
        }

        public string Title { get; private set; }

        public string Note { get; private set; }

        public bool SingleOptionLimitation { get; private set; }

        public DateTime DueDate { get; private set; }

        public IReadOnlyCollection<Option> Options => this.options;

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new DomainException("Title cannot be null or empty");
            }

            this.Title = title;
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate <= SystemTime.Now)
            {
                throw new DomainException("Cannot set due date in the past");
            }

            this.DueDate = dueDate;
        }

        public void AddOption(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                throw new DomainException("Option cannot be null or empty");
            }

            this.options.Add(new Option(option));
        }

        public void Vote(string participantEmailAddress, ICollection<int> optionIds, DateTime votedAt)
        {
            if (this.SingleOptionLimitation && optionIds.Count > 1)
            {
                throw new DomainException("Cannot vote for more than 1 options");
            }

            if (this.DueDate <= SystemTime.Now)
            {
                throw new DomainException("Poll has expired");
            }

            foreach (int optionId in optionIds)
            {
                Option option = this.options.FirstOrDefault(o => o.Id == optionId);

                if (option is null)
                {
                    throw new DomainException($"Option with id {optionId} doesn't exist!");
                }

                option.AddVote(participantEmailAddress, votedAt);
            }
        }
    }
}
