using Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Central.Persistence.Configurations;

public class TenantConfiguration : BaseEntityConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => b.Email)
            .IsUnique();

        builder.Property(t => t.IsActive)
            .HasConversion<bool>();

        builder.Property(t => t.ConnectionString)
            .HasMaxLength(500);

        builder.HasMany(t => t.Domains)
            .WithOne(d => d.Tenant)
            .HasForeignKey(d => d.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Subscriptions)
            .WithOne(bs => bs.Tenant)
            .HasForeignKey(bs => bs.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}