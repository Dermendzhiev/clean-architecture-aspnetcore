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
        private readonly IGetPollInputPort getPollInputPort;
        private readonly GetPollPresenter getPollPresenter;

        private readonly ICreatePollInputPort createPollInputPort;
        private readonly CreatePollPresenter createPollPresenter;

        private readonly IUpdatePollIntputPort updatePollInputPort;
        private readonly UpdatePollPresenter updatePollPresenter;

        private readonly IDeletePollIntputPort deletePollInputPort;
        private readonly DeletePollPresenter deletePollPresenter;

        private readonly IGetVotesInputPort getVotesInputPort;
        private readonly GetVotesPresenter getVotesPresenter;

        private readonly IVoteInputPort voteInputPort;
        private readonly VotePresenter votePresenter;

        public PollController(
            IGetPollInputPort getPollInputPort,
            GetPollPresenter getPollPresenter,
            ICreatePollInputPort createPollInputPort,
            CreatePollPresenter createPollPresenter,
            IUpdatePollIntputPort updatePollInputPort,
            UpdatePollPresenter updatePollPresenter,
            IDeletePollIntputPort deletePollInputPort,
            DeletePollPresenter deletePollPresenter,
            IGetVotesInputPort getVotesInputPort,
            GetVotesPresenter getVotesPresenter,
            IVoteInputPort voteInputPort,
            VotePresenter postVotePresenter)
        {
            this.getPollInputPort = getPollInputPort;
            this.getPollPresenter = getPollPresenter;

            this.createPollInputPort = createPollInputPort;
            this.createPollPresenter = createPollPresenter;

            this.updatePollInputPort = updatePollInputPort;
            this.updatePollPresenter = updatePollPresenter;

            this.deletePollInputPort = deletePollInputPort;
            this.deletePollPresenter = deletePollPresenter;

            this.getVotesInputPort = getVotesInputPort;
            this.getVotesPresenter = getVotesPresenter;

            this.voteInputPort = voteInputPort;
            this.votePresenter = postVotePresenter;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPollResponse>> GetPoll(int id)
        {
            await this.getPollInputPort.HandleAsync(id, this.getPollPresenter);
            return this.getPollPresenter.ViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoll(CreatePollRequest request)
        {
            var createPollInput = new CreatePollInput(request.Title, request.Note, request.SingleOptionLimitation, request.DueDate, request.Options, request.ParticipantEmailAddresses);
            await this.createPollInputPort.HandleAsync(createPollInput, this.createPollPresenter);
            return this.createPollPresenter.ViewModel;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePoll(int id, UpdatePollRequest request)
        {
            var updatePollInput = new UpdatePollInput(id, request.Title, request.DueDate);
            await this.updatePollInputPort.HandleAsync(updatePollInput, this.updatePollPresenter);
            return this.updatePollPresenter.ViewModel;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll(int id)
        {
            await this.deletePollInputPort.HandleAsync(id, this.deletePollPresenter);
            return this.deletePollPresenter.ViewModel;
        }

        [HttpGet("{id}/votes")]
        public async Task<ActionResult<GetVotesResponse>> GetVotes(int id)
        {
            await this.getVotesInputPort.HandleAsync(id, this.getVotesPresenter);
            return this.getVotesPresenter.ViewModel;
        }

        [HttpPatch("{id}/votes")]
        public async Task<IActionResult> Vote(int id, VoteRequest request)
        {
            var createVoteInput = new VoteInput(id, request.ParticipantEmailAddress, request.Options);
            await this.voteInputPort.HandleAsync(createVoteInput, this.votePresenter);
            return this.votePresenter.ViewModel;
        }
    }
}