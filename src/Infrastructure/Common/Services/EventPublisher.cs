using Backend.Application.Common.Events;
using Backend.Shared.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Common.Services;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;
    private readonly IPublisher _mediator;

    public EventPublisher(ILogger<EventPublisher> logger, IPublisher mediator) =>
        (_logger, _mediator) = (logger, mediator);

    public Task PublishAsync(IEvent @event)
    {
        var eventName = @event?.GetType().Name;
        LoggerMessage.Define(LogLevel.Information, new EventId(1, eventName), $"Publishing Event : {eventName}");
        return _mediator.Publish(CreateEventNotification(@event!));
    }

    private static INotification CreateEventNotification(IEvent @event) =>
        (INotification)Activator.CreateInstance(
            typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
}
