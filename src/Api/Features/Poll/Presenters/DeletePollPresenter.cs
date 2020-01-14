namespace CleanArchitecture.Api.Features.Poll.Presenters
{
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using Microsoft.AspNetCore.Mvc;

    public class DeletePollPresenter : IDeletePollOutputPort
    {
        public IActionResult ViewModel { get; private set; }

        public void NotFound(string message) => this.ViewModel = new NotFoundObjectResult(message);

        public void Success() => this.ViewModel = new NoContentResult();
    }
}
