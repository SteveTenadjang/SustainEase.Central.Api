namespace Central.Domain.Entities;

public class TenantDomain : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Tenant? Tenant { get; set; }
}