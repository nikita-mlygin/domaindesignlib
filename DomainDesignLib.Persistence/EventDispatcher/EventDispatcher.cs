namespace DomainDesignLib.Persistence.EventDispatcher;

using DomainDesignLib.Abstractions;
using DomainDesignLib.Abstractions.Event;
using DomainDesignLib.Persistence.EntityTracking;
using MediatR;

public class EventDispatcher(IEntityTracking entityTracking, IMediator mediator) : IEventDispatcher
{
    private readonly IEntityTracking entityTracking = entityTracking;
    private readonly List<DomainEvent> domainEvents = [];
    private readonly IMediator mediator = mediator;

    public Task AddEvent(DomainEvent e)
    {
        domainEvents.Add(e);

        return Task.CompletedTask;
    }

    public Task DispatchEvents()
    {
        entityTracking
            .TrackingEntities.SelectMany(x => x.DomainEvents)
            .Concat(domainEvents)
            .ToList()
            .ForEach(x => mediator.Publish(x));

        return Task.CompletedTask;
    }
}
