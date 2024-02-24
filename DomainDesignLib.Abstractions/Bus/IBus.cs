namespace DomainDesignLib.Abstractions;

public interface IBus
{
    public Task Send<T>(IntegrationEvent<T> integrationEvent);
}
