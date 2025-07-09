using Central.Domain.Events.Tenant;
using Microsoft.Extensions.Logging;

namespace Central.Application.Events.Handlers.Tenant;

public class TenantCreatedEventHandler(ILogger<TenantCreatedEventHandler> logger) : IEventHandler<TenantCreatedEvent>
{
    public async Task Handle(TenantCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Processing TenantCreatedEvent for Tenant: {TenantId} - {TenantName}",
            domainEvent.TenantId, domainEvent.TenantName);

        await Task.Delay(100, cancellationToken);
        logger.LogInformation("Successfully processed TenantCreatedEvent for Tenant: {TenantId}",
            domainEvent.TenantId);
    }
}