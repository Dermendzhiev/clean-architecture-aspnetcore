namespace CleanArchitecture.Api.Features.Poll
{
    using CleanArchitecture.Api.Features.Poll.Models.CreatePoll;
    using CleanArchitecture.Api.Features.Poll.Models.GetPoll;
    using CleanArchitecture.Api.Features.Poll.Models.GetVotes;
    using CleanArchitecture.Api.Features.Poll.Models.UpdatePoll;
    using CleanArchitecture.Api.Features.Poll.Models.Vote;
    using CleanArchitecture.Api.Features.Poll.Presenters;
    using CleanArchitecture.Api.Features.Vote.Presenters;
    using CleanArchitecture.Application.Boundaries.CreatePoll;
    using CleanArchitecture.Application.Boundaries.DeletePoll;
    using CleanArchitecture.Application.Boundaries.GetPoll;
    using CleanArchitecture.Application.Boundaries.GetVotes;
    using CleanArchitecture.Application.Boundaries.UpdatePoll;
    using CleanArchitecture.Application.Boundaries.Vote;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly IGetPollInputBoundary getPollInputBoundary;
        private readonly GetPollPresenter getPollPresenter;

        private readonly ICreatePollInputBoundary createPollInputBoundary;
        private readonly CreatePollPresenter createPollPresenter;

        private readonly IUpdatePollIntputBoundary updatePollInputBoundary;
        private readonly UpdatePollPresenter updatePollPresenter;

        private readonly IDeletePollIntputBoundary deletePollInputBoundary;
        private readonly DeletePollPresenter deletePollPresenter;

        private readonly IGetVotesInputBoundary getVotesInputBoundary;
        private readonly GetVotesPresenter getVotesPresenter;

        private readonly IVoteInputBoundary voteInputBoundary;
        private readonly VotePresenter votePresenter;

        public PollController(
            IGetPollInputBoundary getPollInputBoundary,
            GetPollPresenter getPollPresenter,
            ICreatePollInputBoundary createPollInputBoundary,
            CreatePollPresenter createPollPresenter,
            IUpdatePollIntputBoundary updatePollInputBoundary,
            UpdatePollPresenter updatePollPresenter,
            IDeletePollIntputBoundary deletePollInputBoundary,
            DeletePollPresenter deletePollPresenter,
            IGetVotesInputBoundary getVotesInputBoundary,
            GetVotesPresenter getVotesPresenter,
            IVoteInputBoundary voteInputBoundary,
            VotePresenter postVotePresenter)
        {
            this.getPollInputBoundary = getPollInputBoundary;
            this.getPollPresenter = getPollPresenter;

            this.createPollInputBoundary = createPollInputBoundary;
            this.createPollPresenter = createPollPresenter;

            this.updatePollInputBoundary = updatePollInputBoundary;
            this.updatePollPresenter = updatePollPresenter;

            this.deletePollInputBoundary = deletePollInputBoundary;
            this.deletePollPresenter = deletePollPresenter;

            this.getVotesInputBoundary = getVotesInputBoundary;
            this.getVotesPresenter = getVotesPresenter;

            this.voteInputBoundary = voteInputBoundary;
            this.votePresenter = postVotePresenter;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPollResponse>> GetPoll(int id)
        {
            await this.getPollInputBoundary.HandleAsync(id, this.getPollPresenter);
            return this.getPollPresenter.ViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoll(CreatePollRequest request)
        {
            var createPollInput = new CreatePollInput(request.Title, request.Note, request.SingleOptionLimitation, request.DueDate, request.Options, request.ParticipantEmailAddresses);
            await this.createPollInputBoundary.HandleAsync(createPollInput, this.createPollPresenter);
            return this.createPollPresenter.ViewModel;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePoll(int id, UpdatePollRequest request)
        {
            var updatePollInput = new UpdatePollInput(id, request.Title, request.DueDate);
            await this.updatePollInputBoundary.HandleAsync(updatePollInput, this.updatePollPresenter);
            return this.updatePollPresenter.ViewModel;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll(int id)
        {
            await this.deletePollInputBoundary.HandleAsync(id, this.deletePollPresenter);
            return this.deletePollPresenter.ViewModel;
        }

        [HttpGet("{id}/votes")]
        public async Task<ActionResult<GetVotesResponse>> GetVotes(int id)
        {
            await this.getVotesInputBoundary.HandleAsync(id, this.getVotesPresenter);
            return this.getVotesPresenter.ViewModel;
        }

        [HttpPatch("{id}/votes")]
        public async Task<IActionResult> Vote(int id, VoteRequest request)
        {
            var createVoteInput = new VoteInput(id, request.ParticipantEmailAddress, request.Options);
            await this.voteInputBoundary.HandleAsync(createVoteInput, this.votePresenter);
            return this.votePresenter.ViewModel;
        }
    }
}