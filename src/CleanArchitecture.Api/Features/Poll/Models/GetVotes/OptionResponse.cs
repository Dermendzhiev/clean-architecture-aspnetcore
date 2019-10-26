namespace CleanArchitecture.Api.Features.Poll.Models.GetVotes
{
    using System.Linq;
    using System.Collections.Generic;

    public class OptionResponse
    {
        public string Option { get; set; }

        public int Votes => this.ParticipantEmailAddresss.Count();

        public ICollection<string> ParticipantEmailAddresss { get; set; }
    }
}
