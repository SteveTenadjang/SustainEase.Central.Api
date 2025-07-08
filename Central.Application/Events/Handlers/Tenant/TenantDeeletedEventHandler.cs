using Central.Domain.Events.Tenant;
using Microsoft.Extensions.Logging;

namespace Central.Application.Events.Handlers.Tenant;

public class TenantDeletedEventHandler(ILogger<TenantCreatedEventHandler> logger) : IEventHandler<TenantDeletedEvent>
{
    public async Task Handle(TenantDeletedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Processing TenantDeletedEvent for Tenant: {TenantId} - {TenantName}", 
            domainEvent.TenantId, domainEvent.TenantName);

        await Task.Delay(100, cancellationToken);
        logger.LogInformation("Successfully processed TenantDeletedEvent for Tenant: {TenantId}", 
            domainEvent.TenantId);
    }
}