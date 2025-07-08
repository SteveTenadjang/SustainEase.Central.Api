using Central.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Central.Persistence.Configurations;

public class BundleConfiguration : BaseEntityConfiguration<Bundle>
{
    public override void Configure(EntityTypeBuilder<Bundle> builder)
    {
        base.Configure(builder);
        
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Key)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => b.Key)
            .IsUnique();

        builder.HasIndex(b => b.Name)
            .IsUnique();

        builder.Property(b => b.Description)
            .HasMaxLength(500);
    }
}