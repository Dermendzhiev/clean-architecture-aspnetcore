namespace CleanArchitecture.IntegrationTests.Repositories
{
    using System;
    using System.Linq;
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Persistence;
    using CleanArchitecture.Persistence.Repositories;
    using FluentAssertions;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class PollGatewayTests
    {
        [Fact]
        public void Add_Poll_To_Database()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<CleanArchitectureDbContext> options = new DbContextOptionsBuilder<CleanArchitectureDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new CleanArchitectureDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                string title = "test title";
                Poll poll = this.GetDefaultPoll(title);

                // Run the test against one instance of the context
                using (var context = new CleanArchitectureDbContext(options))
                {
                    var pollGateway = new PollGateway(context);
                    pollGateway.CreateAsync(poll);
                    context.SaveChanges();
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new CleanArchitectureDbContext(options))
                {
                    context.Polls.Count().Should().Be(1);
                    context.Polls.Single().Title.Should().Be("test title");
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private Poll GetDefaultPoll(string title = "test")
        {
            const string note = "test note";
            bool singleOptionLimitation = false;
            DateTime dueDate = DateTime.Now.AddDays(1);

            return new Poll(title, note, singleOptionLimitation, dueDate);
        }
    }
}
