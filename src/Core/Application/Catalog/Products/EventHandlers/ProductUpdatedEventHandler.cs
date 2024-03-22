using Backend.Application.Common.Events;
using Backend.Domain.Catalog;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.EventHandlers;

public class ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger) : EventNotificationHandler<EntityUpdatedEvent<Product>>
{
    public override Task Handle(EntityUpdatedEvent<Product> @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}
