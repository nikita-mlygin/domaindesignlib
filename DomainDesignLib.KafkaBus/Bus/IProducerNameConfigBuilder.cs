namespace DomainDesignLib.KafkaBus;

public interface IProducerNameConfigBuilder
{
    public IProducerNameConfigBuilder AddName<T>(string name);
    public IProducerNameConfigBuilder AddName(Type producerType, string name);
}
