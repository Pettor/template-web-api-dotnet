using Backend.Application.Common.Interfaces;
using Backend.Shared.Notifications;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.SignalR;
using static Backend.Shared.Notifications.NotificationConstants;

namespace Backend.Infrastructure.Notifications;

public class NotificationSender(
    IHubContext<NotificationHub> notificationHubContext,
    ITenantInfo currentTenant
) : INotificationSender
{
    public Task BroadcastAsync(
        INotificationMessage notification,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext.Clients.All.SendAsync(
            NotificationFromServer,
            notification.GetType().FullName,
            notification,
            cancellationToken
        );

    public Task BroadcastAsync(
        INotificationMessage notification,
        IEnumerable<string> excludedConnectionIds,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.AllExcept(excludedConnectionIds)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToAllAsync(
        INotificationMessage notification,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.Group($"GroupTenant-{currentTenant.Id}")
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToAllAsync(
        INotificationMessage notification,
        IEnumerable<string> excludedConnectionIds,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.GroupExcept($"GroupTenant-{currentTenant.Id}", excludedConnectionIds)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToGroupAsync(
        INotificationMessage notification,
        string group,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.Group(group)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToGroupAsync(
        INotificationMessage notification,
        string group,
        IEnumerable<string> excludedConnectionIds,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.GroupExcept(group, excludedConnectionIds)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToGroupsAsync(
        INotificationMessage notification,
        IEnumerable<string> groupNames,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.Groups(groupNames)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToUserAsync(
        INotificationMessage notification,
        string userId,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.User(userId)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );

    public Task SendToUsersAsync(
        INotificationMessage notification,
        IEnumerable<string> userIds,
        CancellationToken cancellationToken
    ) =>
        notificationHubContext
            .Clients.Users(userIds)
            .SendAsync(
                NotificationFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken
            );
}
