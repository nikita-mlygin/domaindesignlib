using KafkaFlow.Configuration;

namespace DomainDesignLib.KafkaBus;

public class ProducerNameConfigurationBuilder : IProducerNameConfigBuilder
{
    private readonly ProducerNameConfig producerNameConfig = new();

    public IProducerNameConfigBuilder AddName<T>(string name)
    {
        return AddName(typeof(T), name);
    }

    public IProducerNameConfigBuilder AddName(Type producerType, string name)
    {
        producerNameConfig.ProducerNames.Add(producerType, name);

        return this;
    }

    public ProducerNameConfig Build()
    {
        return this.producerNameConfig;
    }
}
