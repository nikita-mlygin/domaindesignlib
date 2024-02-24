namespace DomainDesignLib.KafkaBus;

using DomainDesignLib.Abstractions;
using KafkaFlow.Configuration;
using KafkaFlow.Producers;

public class KafkaBus(
    IProducerAccessor producerAccessor,
    IProducerNameResolver producerNameResolver
) : IBus
{
    private readonly IProducerNameResolver producerNameResolver = producerNameResolver;
    private readonly IProducerAccessor producerAccessor = producerAccessor;

    public async Task Send<T>(IntegrationEvent<T> integrationEvent)
    {
        var name = producerNameResolver.GetName(integrationEvent.GetType());

        var producer = producerAccessor.GetProducer(name);

        await producer.ProduceAsync(integrationEvent.Id.ToString(), integrationEvent.Message);
    }
}
