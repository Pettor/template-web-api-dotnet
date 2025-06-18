using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Notifications;

[Authorize]
public class NotificationHub(ITenantInfo? currentTenant, ILogger<NotificationHub> logger)
    : Hub,
        ITransientService
{
    public override async Task OnConnectedAsync()
    {
        if (currentTenant is null)
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupTenant-{currentTenant.Id}");

        await base.OnConnectedAsync();

        logger.LogInformation(
            "A client connected to NotificationHub: {connectionId}",
            Context.ConnectionId
        );
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"GroupTenant-{currentTenant!.Id}");

        await base.OnDisconnectedAsync(exception);

        logger.LogInformation(
            "A client disconnected from NotificationHub: {connectionId}",
            Context.ConnectionId
        );
    }
}
