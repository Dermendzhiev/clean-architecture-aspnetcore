namespace CleanArchitecture.Api.Features.Poll.Models.CreatePoll
{
    using System;
    using System.Collections.Generic;

    public class CreatePollRequest
    {
        public string Title { get; set; }

        public string Note { get; set; }

        public bool SingleOptionLimitation { get; set; }

        public DateTime DueDate { get; set; }

        public List<string> Options { get; set; }

        /// <summary>
        /// Participant to be notified about the new poll
        /// </summary>
        public List<string> ParticipantEmailAddresses { get; set; }
    }
}
