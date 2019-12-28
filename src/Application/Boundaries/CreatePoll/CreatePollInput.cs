namespace CleanArchitecture.Application.Boundaries.CreatePoll
{
    using System;
    using System.Collections.Generic;

    public class CreatePollInput
    {
        public CreatePollInput(string title, string note, bool singleOptionLimitation, DateTime dueDate, List<string> options, List<string> participantEmailAddresses)
        {
            this.Title = title;
            this.Note = note;
            this.SingleOptionLimitation = singleOptionLimitation;
            this.DueDate = dueDate;
            this.Options = options;
            this.ParticipantEmailAddresses = participantEmailAddresses;
        }

        public string Title { get; set; }

        public string Note { get; set; }

        public bool SingleOptionLimitation { get; set; }

        public DateTime DueDate { get; set; }

        public List<string> Options { get; set; }

        public List<string> ParticipantEmailAddresses { get; set; }
    }
}
