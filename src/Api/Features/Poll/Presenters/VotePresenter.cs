namespace CleanArchitecture.Api.Features.Vote.Presenters
{
    using CleanArchitecture.Application.Boundaries.Vote;
    using Microsoft.AspNetCore.Mvc;

    public class VotePresenter : IVoteOutputPort
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