namespace GregCustomers.Application.Abstractions.Persistence;

public interface IAddressCommands
{
    Task CreateAsync(Guid id, Guid clientId, string street, CancellationToken ct);
    Task UpdateAsync(Guid id, string street, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}