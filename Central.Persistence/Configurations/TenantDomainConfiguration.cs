using Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Central.Persistence.Configurations;

public class TenantDomainConfiguration : BaseEntityConfiguration<TenantDomain>
{
    public override void Configure(EntityTypeBuilder<TenantDomain> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(td => td.Tenant)
            .WithMany(t => t.Domains)
            .HasForeignKey(td => td.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}