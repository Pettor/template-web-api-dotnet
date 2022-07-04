using WebApiTemplate.Shared.Events;

namespace WebApiTemplate.Domain.Common.Contracts;

public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}