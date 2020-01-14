namespace CleanArchitecture.Api.Features.Poll.Presenters
{
    using CleanArchitecture.Application.Boundaries.UpdatePoll;
    using Microsoft.AspNetCore.Mvc;

    public class UpdatePollPresenter : IUpdatePollOutputPort
    {
        public IActionResult ViewModel { get; private set; }

        public void NotFound(string message) => this.ViewModel = new NotFoundObjectResult(message);

        public void Error(string message)
        {
            this.ViewModel = new BadRequestObjectResult(new
            {
                ErrorMessage = message
            });
        }

        public void Success() => this.ViewModel = new NoContentResult();
    }
}
