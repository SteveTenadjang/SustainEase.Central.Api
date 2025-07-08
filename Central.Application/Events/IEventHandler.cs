using Central.Domain.Events;

namespace Central.Application.Events;

public interface IEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}