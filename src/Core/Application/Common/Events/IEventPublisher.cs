using MyHero.Shared.Events;

namespace MyHero.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}