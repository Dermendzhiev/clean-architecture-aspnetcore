namespace CleanArchitecture.Application.Boundaries.GetPoll
{
    using System;
    using System.Collections.Generic;
    using CleanArchitecture.Domain.Entities;

    public class GetPollOutput
    {
        public GetPollOutput(int id, string title, string note, DateTime dueDate, bool singleOptionLimitation, IReadOnlyCollection<Option> options)
        {
            this.Id = id;
            this.Title = title;
            this.Note = note;
            this.DueDate = dueDate;
            this.SingleOptionLimitation = singleOptionLimitation;
            this.Options = options;
        }

        public int Id { get; }

        public string Title { get; }

        public string Note { get; }

        public DateTime DueDate { get; }

        public bool SingleOptionLimitation { get; }

        public IReadOnlyCollection<Option> Options { get; }
    }
}
