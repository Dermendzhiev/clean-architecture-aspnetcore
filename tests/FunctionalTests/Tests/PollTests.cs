namespace CleanArchitecture.FunctionalTests.Tests
{
    using Xunit;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CleanArchitecture.FunctionalTests.Helpers;
    using System.Linq;
    using FluentAssertions;
    using System.Net;
    using Newtonsoft.Json;
    using CleanArchitecture.Api.Features.Poll.Models.GetPoll;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Api.Features.Poll.Models.CreatePoll;
    using System.Collections.Generic;
    using System.Text;
    using CleanArchitecture.Api.Features.Poll.Models.UpdatePoll;
    using CleanArchitecture.Api.Features.Poll.Models.GetVotes;
    using CleanArchitecture.Api.Features.Poll.Models.Vote;
    using CleanArchitecture.Api;
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using CleanArchitecture.Persistence;

    public class PollTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public PollTests(CustomWebApplicationFactory<Startup> factory) => this.factory = factory;

        [Fact]
        public async Task GetPoll_ReturnsSpecifiedPoll()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            int pollId = PredefinedData.Polls.First().Id;

            // Act
            HttpResponseMessage response = await client.GetAsync($"/api/Poll/{pollId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseString = await response.Content.ReadAsStringAsync();
            GetPollResponse poll = JsonConvert.DeserializeObject<GetPollResponse>(responseString);
            poll.Id.Should().Be(pollId);
        }

        [Fact]
        public async Task CreatePoll_ReturnsCreatedWithPollId()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            var createPollRequest = new CreatePollRequest
            {
                Title = "Test",
                Note = "test",
                DueDate = DateTime.Now.AddDays(1),
                SingleOptionLimitation = true,
                Options = new List<string>
            {
                "one",
                "two"
            },
                ParticipantEmailAddresses = new List<string>
            {
                "email@test.com"
            }
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(createPollRequest), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PostAsync("/api/Poll", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            string responseString = await response.Content.ReadAsStringAsync();
            int actualPollId = JsonConvert.DeserializeObject<int>(responseString);
            actualPollId.Should().NotBe(0);
        }

        [Fact]
        public async Task UpdatePoll_ReturnsNoContent()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            int pollId = PredefinedData.Polls.First().Id;

            var updatePollRequest = new UpdatePollRequest
            {
                Title = "Test 2",
                DueDate = DateTime.Now.AddDays(2)
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(updatePollRequest), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PatchAsync($"/api/Poll/{pollId}", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeletePoll_ReturnsNoContent()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            int pollId = PredefinedData.Polls.First().Id;

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"/api/Poll/{pollId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetVotes_ReturnsVotes()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            Poll poll = PredefinedData.Polls.First();

            var voteRequest = new VoteRequest
            {
                ParticipantEmailAddress = "test1@test.com",
                Options = new List<int>
            {
                poll.Options.ElementAt(0).Id,
                poll.Options.ElementAt(1).Id
            }
            };
            var voteHttpContent = new StringContent(JsonConvert.SerializeObject(voteRequest), Encoding.UTF8, "application/json");
            await client.PatchAsync($"/api/Poll/{poll.Id}/votes", voteHttpContent);

            // Act
            HttpResponseMessage response = await client.GetAsync($"/api/Poll/{poll.Id}/votes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string responseString = await response.Content.ReadAsStringAsync();
            GetVotesResponse getVotesResponse = JsonConvert.DeserializeObject<GetVotesResponse>(responseString);
            int totalVotes = getVotesResponse.Options.Sum(option => option.Votes);
            totalVotes.Should().Be(2);
        }

        [Fact]
        public async Task Vote_ReturnsNoContent()
        {
            // Arrange
            HttpClient client = this.factory.CreateClient();
            Poll poll = PredefinedData.Polls.First();
            var voteRequest = new VoteRequest
            {
                ParticipantEmailAddress = "test1@test.com",
                Options = new List<int>
            {
                poll.Options.ElementAt(0).Id,
                poll.Options.ElementAt(1).Id
            }
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(voteRequest), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PatchAsync($"/api/Poll/{poll.Id}/votes", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public void Dispose()
        {
            using IServiceScope scope = this.factory.Server.Services.CreateScope();

            IServiceProvider scopedServices = scope.ServiceProvider;
            CleanArchitectureDbContext dbContext = scopedServices.GetRequiredService<CleanArchitectureDbContext>();
            DatabaseInitializer.ReinitializeDbForTests(dbContext);
        }
    }
}
