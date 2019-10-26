namespace CleanArchitecture.Api.Features.Poll.Presenters
{
    using CleanArchitecture.Application.Boundaries.CreatePoll;
    using Microsoft.AspNetCore.Mvc;

    public class CreatePollPresenter : ICreatePollOutputBoundary
    {
        public IActionResult ViewModel { get; private set; }

        public void Error(string message)
        {
            this.ViewModel = new BadRequestObjectResult(new
            {
                ErrorMessage = message
            });
        }

        public void Success(CreatePollOutput output) => this.ViewModel = new CreatedResult(string.Empty, output.Id);
    }
}
