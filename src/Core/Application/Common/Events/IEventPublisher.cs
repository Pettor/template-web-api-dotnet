using Backend.Application.Common.Interfaces;
using Backend.Shared.Events;

namespace Backend.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}
