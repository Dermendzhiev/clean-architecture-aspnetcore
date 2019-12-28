namespace CleanArchitecture.Api.Features.Poll.Models.GetPoll
{
    using System;
    using System.Collections.Generic;

    public class GetPollResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public DateTime DueDate { get; set; }

        public bool SingleOptionLimitation { get; set; }

        public List<OptionResponse> Options { get; set; }
    }
}
