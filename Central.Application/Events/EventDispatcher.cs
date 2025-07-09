using Central.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Central.Application.Events;

public class EventDispatcher(IServiceProvider serviceProvider, ILogger<EventDispatcher> logger)
    : IEventDispatcher
{
    public async Task DispatchAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        logger.LogInformation("Dispatching domain event: {EventType} with ID: {EventId}", typeof(T).Name,
            domainEvent.Id);

        var handlers = serviceProvider.GetServices<IEventHandler<T>>().ToList();

        if (handlers.Count == 0)
        {
            logger.LogWarning("No handlers found for event type: {EventType}", typeof(T).Name);
            return;
        }

        var tasks = handlers.Select(handler => HandleEventSafely(handler, domainEvent, cancellationToken));
        await Task.WhenAll(tasks);
        logger.LogInformation("Successfully dispatched domain event: {EventType}", typeof(T).Name);
    }

    private async Task HandleEventSafely<T>(IEventHandler<T> handler, T domainEvent,
        CancellationToken cancellationToken)
        where T : IDomainEvent
    {
        await handler.Handle(domainEvent, cancellationToken);
        logger.LogDebug("Handler {HandlerType} successfully processed event {EventType}", handler.GetType().Name,
            typeof(T).Name);
    }
}