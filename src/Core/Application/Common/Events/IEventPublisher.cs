using WebApiTemplate.Application.Common.Interfaces;
using WebApiTemplate.Shared.Events;

namespace WebApiTemplate.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}