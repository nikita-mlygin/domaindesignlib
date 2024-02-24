namespace DomainDesignLib.KafkaBus;

public class ProducerNameConfig
{
    private readonly Dictionary<Type, string> producerNames = [];

    public Dictionary<Type, string> ProducerNames
    {
        get => producerNames;
    }
}
