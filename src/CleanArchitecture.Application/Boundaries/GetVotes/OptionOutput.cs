namespace CleanArchitecture.Application.Boundaries.GetVotes
{
    using System.Collections.Generic;

    public class OptionOutput
    {
        public string Option { get; set; }

        public ICollection<string> ParticipantEmailAddresss { get; set; }
    }
}
