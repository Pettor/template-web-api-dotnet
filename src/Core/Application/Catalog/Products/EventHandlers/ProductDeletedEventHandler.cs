using WebApiTemplate.Application.Common.Events;
using WebApiTemplate.Domain.Catalog;
using WebApiTemplate.Domain.Common.Events;

namespace WebApiTemplate.Application.Catalog.Products.EventHandlers;

public class ProductDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Product>>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}