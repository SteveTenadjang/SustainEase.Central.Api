using Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Central.Persistence.Configurations;

public class TenantSubscriptionConfiguration :  BaseEntityConfiguration<TenantSubscription>
{
    public override void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        base.Configure(builder);
        
        builder.Property(b => b.TenantId)
            .HasConversion<Guid>()
            .IsRequired();

        builder.Property(b => b.BundleId)
            .HasConversion<Guid>()
            .IsRequired();

        builder.Property(b => b.Duration)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(b => b.StartDate)
            .IsRequired();

        builder.HasOne(ts => ts.Tenant)
            .WithMany(t => t.Subscriptions)
            .HasForeignKey(ts => ts.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ts => ts.Bundle)
            .WithMany()
            .HasForeignKey(ts => ts.BundleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}