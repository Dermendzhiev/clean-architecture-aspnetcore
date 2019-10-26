namespace CleanArchitecture.Persistence.EntityConfigurations
{
    using CleanArchitecture.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OptionEntityTypeConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> optionConfiguration)
        {
            optionConfiguration.ToTable("Options", CleanArchitectureDbContext.DEFAULT_SCHEMA);

            optionConfiguration.HasKey(option => option.Id);

            optionConfiguration.Property(option => option.Text)
                .IsRequired();

            optionConfiguration.Property<int>("PollId")
                .IsRequired();

            optionConfiguration.HasMany(option => option.Votes)
               .WithOne()
               .HasForeignKey("OptionId")
               .OnDelete(DeleteBehavior.Cascade);

            IMutableNavigation navigation = optionConfiguration.Metadata.FindNavigation(nameof(Option.Votes));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
