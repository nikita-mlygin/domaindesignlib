namespace DomainDesignLib.Abstractions.Event;

public interface IEventDispatcher
{
    public Task DispatchEvents();
    public Task AddEvent(DomainEvent e);
}
