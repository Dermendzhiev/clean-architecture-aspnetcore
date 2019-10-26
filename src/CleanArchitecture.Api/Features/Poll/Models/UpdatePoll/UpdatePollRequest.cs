namespace CleanArchitecture.Api.Features.Poll.Models.UpdatePoll
{
    using System;

    public class UpdatePollRequest
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }
    }
}
