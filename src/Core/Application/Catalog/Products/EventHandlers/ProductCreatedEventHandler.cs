using Backend.Application.Common.Events;
using Backend.Domain.Catalog;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Product>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}