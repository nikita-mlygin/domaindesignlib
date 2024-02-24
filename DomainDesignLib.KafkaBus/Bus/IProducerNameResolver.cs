namespace DomainDesignLib.KafkaBus;

public interface IProducerNameResolver
{
    public string GetName(Type messageType);
    public string GetName<T>();
}
