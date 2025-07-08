namespace Central.Domain.Entities;

public class TenantSubscription : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid BundleId { get; set; }
    public int Duration { get; set; }
    public DateTime StartDate { get; set; }

    public Tenant? Tenant { get; set; }
    public Bundle? Bundle { get; set; }
}