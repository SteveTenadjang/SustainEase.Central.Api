using Central.Domain.Events;

namespace Central.Application.Events;

public interface IEventDispatcher
{
    Task DispatchAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}