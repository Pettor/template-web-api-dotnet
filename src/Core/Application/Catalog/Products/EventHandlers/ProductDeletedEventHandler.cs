using Backend.Application.Common.Events;
using Backend.Domain.Catalog;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.EventHandlers;

public class ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger) : EventNotificationHandler<EntityDeletedEvent<Product>>
{
    public override Task Handle(EntityDeletedEvent<Product> @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}
