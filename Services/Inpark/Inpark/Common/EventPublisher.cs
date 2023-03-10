using Microsoft.Extensions.Logging;

namespace Zeta.Inpark.Common;

public interface IEventPublisher
{
    Task Publish(DomainEvent domainEvent);
}

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;
    private readonly IPublisher _mediator;

    public EventPublisher(ILogger<EventPublisher> logger, IPublisher mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        _logger.LogInformation(
            "Publishing domain event. Event - {Event} with the contents {Contents}", 
            domainEvent.GetType().Name,
            domainEvent.ToString()
        );

        domainEvent.Publish();

        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()),
            domainEvent
        )!;
    }
}