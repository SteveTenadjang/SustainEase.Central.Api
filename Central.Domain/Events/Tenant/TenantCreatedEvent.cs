
namespace Central.Domain.Events.Tenant;

public class TenantCreatedEvent(Guid tenantId, string tenantName, string email) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid TenantId { get; } = tenantId;
    public string TenantName { get; } = tenantName;
    public string Email { get; } = email;
}