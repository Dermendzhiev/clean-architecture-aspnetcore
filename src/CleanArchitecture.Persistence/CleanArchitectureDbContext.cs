namespace CleanArchitecture.Persistence
{
    using CleanArchitecture.Domain.Entities;
    using CleanArchitecture.Persistence.EntityConfigurations;
    using Microsoft.EntityFrameworkCore;

    public class CleanArchitectureDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "polls";

        public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options)
            : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PollEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OptionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoteEntityTypeConfiguration());
        }
    }
}
