namespace DomainDesignLib.Abstractions.Entity;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}
