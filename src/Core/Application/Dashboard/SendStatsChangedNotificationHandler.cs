using Backend.Application.Common.Events;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Catalog;
using Backend.Domain.Common.Events;
using Backend.Domain.Identity;
using Backend.Shared.Events;
using Backend.Shared.Notifications;

namespace Backend.Application.Dashboard;

public class SendStatsChangedNotificationHandler(ILogger<SendStatsChangedNotificationHandler> logger, INotificationSender notifications)
    :
        IEventNotificationHandler<EntityCreatedEvent<Brand>>,
        IEventNotificationHandler<EntityDeletedEvent<Brand>>,
        IEventNotificationHandler<EntityCreatedEvent<Product>>,
        IEventNotificationHandler<EntityDeletedEvent<Product>>,
        IEventNotificationHandler<ApplicationRoleCreatedEvent>,
        IEventNotificationHandler<ApplicationRoleDeletedEvent>,
        IEventNotificationHandler<ApplicationUserCreatedEvent>
{
    public Task Handle(EventNotification<EntityCreatedEvent<Brand>> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<EntityDeletedEvent<Brand>> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<EntityCreatedEvent<Product>> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<EntityDeletedEvent<Product>> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationRoleCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationRoleDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationUserCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    private Task SendStatsChangedNotification(IEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("{event} Triggered => Sending StatsChangedNotification", @event.GetType().Name);

        return notifications.SendToAllAsync(new StatsChangedNotification(), cancellationToken);
    }
}
