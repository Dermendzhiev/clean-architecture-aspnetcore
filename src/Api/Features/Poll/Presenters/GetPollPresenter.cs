namespace CleanArchitecture.Api.Features.Poll.Presenters
{
    using System.Linq;
    using CleanArchitecture.Api.Features.Poll.Models.GetPoll;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using Microsoft.AspNetCore.Mvc;

    public class GetPollPresenter : IGetPollOutputPort
    {
        public ActionResult<GetPollResponse> ViewModel { get; private set; }

        public void NotFound(string message) => this.ViewModel = new NotFoundObjectResult(message);

        public void Success(GetPollOutput output)
        {
            var optionsResponse = output.Options
                .Select(option => new OptionResponse
                {
                    Id = option.Id,
                    Option = option.Text
                })
                .ToList();

            var response = new GetPollResponse
            {
                Id = output.Id,
                Title = output.Title,
                Note = output.Note,
                DueDate = output.DueDate,
                SingleOptionLimitation = output.SingleOptionLimitation,
                Options = optionsResponse
            };

            this.ViewModel = new OkObjectResult(response);
        }
    }
}
