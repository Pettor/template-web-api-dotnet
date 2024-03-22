using Backend.Application.Common.Events;
using Backend.Domain.Catalog;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) : EventNotificationHandler<EntityCreatedEvent<Product>>
{
    public override Task Handle(EntityCreatedEvent<Product> @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}
