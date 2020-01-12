namespace CleanArchitecture.Persistence.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Interfaces.Gateways;
    using CleanArchitecture.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class PollGateway : IPollGateway
    {
        private readonly CleanArchitectureDbContext dbContext;

        public PollGateway(CleanArchitectureDbContext dbContext) => this.dbContext = dbContext;

        public Task CreateAsync(Poll poll)
        {
            this.dbContext.Polls.Add(poll);
            return this.dbContext.SaveChangesAsync();
        }

        public Task<Poll> GetAsync(int id)
        {
            return this.dbContext.Polls
                .AsQueryable()
                .AsNoTracking()
                .Include(poll => poll.Options).ThenInclude(option => option.Votes)
                .FirstOrDefaultAsync(poll => poll.Id == id);
        }

        public Task UpdateAsync(Poll poll)
        {
            this.dbContext.Entry(poll).State = EntityState.Modified;

            foreach (Option option in poll.Options)
            {
                if (option.Id == 0)
                {
                    this.dbContext.Entry(option).State = EntityState.Added;
                }
                else
                {
                    this.dbContext.Entry(option).State = EntityState.Modified;
                    foreach (Vote vote in option.Votes)
                    {
                        if (vote.Id == 0)
                        {
                            this.dbContext.Entry(vote).State = EntityState.Added;
                        }
                        else
                        {
                            this.dbContext.Entry(vote).State = EntityState.Modified;
                        }
                    }
                }
            }

            return this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Poll poll)
        {
            this.dbContext.Entry(poll).State = EntityState.Deleted;
            await this.dbContext.SaveChangesAsync();
        }
    }
}