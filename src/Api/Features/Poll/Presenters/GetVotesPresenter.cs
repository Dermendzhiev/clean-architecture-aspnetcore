namespace CleanArchitecture.Api.Features.Poll.Presenters
{
    using System.Linq;
    using CleanArchitecture.Api.Features.Poll.Models.GetVotes;
    using CleanArchitecture.Application.Boundaries.GetVotes;
    using Microsoft.AspNetCore.Mvc;

    public class GetVotesPresenter : IGetVotesOutputPort
    {
        public ActionResult<GetVotesResponse> ViewModel { get; private set; }

        public void NotFound(string message) => this.ViewModel = new NotFoundObjectResult(message);

        public void Error(string message)
        {
            this.ViewModel = new BadRequestObjectResult(new
            {
                ErrorMessage = message
            });
        }

        public void Success(GetVotesOutput output)
        {
            var optionsResponse = output.Options
                .Select(optionOutput => new OptionResponse
                {
                    Option = optionOutput.Option,
                    ParticipantEmailAddresss = optionOutput.ParticipantEmailAddresss
                })
                .OrderBy(o => o.Votes)
                .ToList();

            var response = new GetVotesResponse
            {
                Title = output.Title,
                Options = optionsResponse
            };

            this.ViewModel = new OkObjectResult(response);
        }
    }
}