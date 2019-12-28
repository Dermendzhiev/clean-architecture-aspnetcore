namespace CleanArchitecture.Application.Boundaries.UpdatePoll
{
    using System;

    public class UpdatePollInput
    {
        public UpdatePollInput(int id, string title, DateTime dueDate)
        {
            this.Id = id;
            this.Title = title;
            this.DueDate = dueDate;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DueDate { get; set; }
    }
}
