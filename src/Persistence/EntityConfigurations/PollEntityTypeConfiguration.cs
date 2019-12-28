namespace CleanArchitecture.Persistence.EntityConfigurations
{
    using CleanArchitecture.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PollEntityTypeConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> pollConfiguration)
        {
            pollConfiguration.ToTable("Polls", CleanArchitectureDbContext.DEFAULT_SCHEMA);

            pollConfiguration.HasKey(poll => poll.Id);

            pollConfiguration.Property(poll => poll.Title)
                .IsRequired();

            pollConfiguration.HasMany(poll => poll.Options)
               .WithOne()
               .HasForeignKey("PollId")
               .OnDelete(DeleteBehavior.Cascade);

            // Set as field (New since EF 1.1) to access the Option collection property through its field
            IMutableNavigation navigation = pollConfiguration.Metadata.FindNavigation(nameof(Poll.Options));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
