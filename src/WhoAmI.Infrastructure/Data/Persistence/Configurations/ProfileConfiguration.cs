using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Infrastructure.Data.Persistence.Configurations.Constraints;

namespace WhoAmI.Infrastructure.Data.Persistence.Configurations;

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile> {
    public void Configure(EntityTypeBuilder<Profile> builder) {
        ConfigureToTable(builder);
        ConfigureProperties(builder);
        ConfigureKey(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureToTable(EntityTypeBuilder<Profile> builder) =>
        builder.ToTable("Profiles");

    private static void ConfigureKey(EntityTypeBuilder<Profile> builder) =>
        builder.HasKey(p => p.Id);

    private static void ConfigureIndexes(EntityTypeBuilder<Profile> builder) {
        builder
            .HasIndex(p => p.Id)
            .IsUnique()
            .HasDatabaseName(DatabaseConstraintNames.ProfileGuid);

        builder
            .HasIndex(p => p.Email)
            .IsUnique()
            .HasDatabaseName(DatabaseConstraintNames.ProfileEmail);
    }

    private static void ConfigureProperties(
        EntityTypeBuilder<Profile> builder
    ) {
        ConfigureId(builder);
        ConfigureCreatedAt(builder);
        ConfigureUpdatedAt(builder);
        ConfigureFullName(builder);
        ConfigureEmail(builder);
        ConfigureLinkedIn(builder);
        ConfigureGitHub(builder);
    }

    private static void ConfigureId(EntityTypeBuilder<Profile> builder) =>
        builder
            .Property(p => p.Id)
            .HasColumnName("id")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v))
            .IsRequired();

    private static void ConfigureCreatedAt(
        EntityTypeBuilder<Profile> builder
    ) =>
        builder
            .Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

    private static void ConfigureUpdatedAt(
        EntityTypeBuilder<Profile> builder
    ) =>
        builder
            .Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

    private static void ConfigureFullName(EntityTypeBuilder<Profile> builder) {
        builder.OwnsOne(p => p.FullName, fullName => {
            fullName.OwnsOne(fn => fn.FirstName, firstName => {
                firstName
                    .Property(f => f.Value)
                    .HasColumnName("first_name")
                    .HasMaxLength(FirstName.MaxLength)
                    .IsRequired();
            });

            fullName.OwnsOne(fn => fn.LastName, lastName => {
                lastName
                    .Property(l => l.Value)
                    .HasColumnName("last_name")
                    .HasMaxLength(LastName.MaxLength)
                    .IsRequired();
            });
        });
    }

    private static void ConfigureEmail(EntityTypeBuilder<Profile> builder) =>
        builder
            .Property(p => p.Email)
            .HasConversion(v => v.Address, v => new Email(v))
            .HasColumnName("email")
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

    private static void ConfigureLinkedIn(
        EntityTypeBuilder<Profile> builder
    ) =>
        ConfigureUrl(builder, p => p.LinkedIn, "linkedin_url");

    private static void ConfigureGitHub(EntityTypeBuilder<Profile> builder) =>
        ConfigureUrl(builder, p => p.GitHub, "github_url");

    private static void ConfigureUrl(
        EntityTypeBuilder<Profile> builder,
        Expression<Func<Profile, Url>> property,
        string columnName
    ) =>
        builder
            .Property(property)
            .HasColumnName(columnName)
            .HasConversion(v => v.Value, v => new Url(v))
            .IsRequired();
}
