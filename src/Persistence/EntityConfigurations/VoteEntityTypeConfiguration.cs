namespace CleanArchitecture.Persistence.EntityConfigurations
{
    using CleanArchitecture.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VoteEntityTypeConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> voteConfiguration)
        {
            voteConfiguration.ToTable("Votes", CleanArchitectureDbContext.DEFAULT_SCHEMA);

            voteConfiguration.HasKey(vote => vote.Id);

            voteConfiguration.Property<int>("OptionId")
                .IsRequired();
        }
    }
}
