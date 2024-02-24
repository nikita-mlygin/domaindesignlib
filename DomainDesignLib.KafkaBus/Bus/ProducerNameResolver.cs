using KafkaFlow.Configuration;

namespace DomainDesignLib.KafkaBus;

public class ProducerNameResolver(ProducerNameConfig producerNameConfig) : IProducerNameResolver
{
    private readonly ProducerNameConfig producerNameConfig = producerNameConfig;

    public string GetName(Type messageType)
    {
        if (producerNameConfig.ProducerNames.TryGetValue(messageType, out var name))
        {
            return name;
        }

        throw new ArgumentOutOfRangeException(
            nameof(messageType),
            "The producer name for this event not founded"
        );
    }

    public string GetName<T>()
    {
        return GetName(typeof(T));
    }
}
