namespace GregCustomers.Application.Abstractions.Persistence;

public interface IClientCommands
{
    Task CreateAsync(Guid id, string name, string email, CancellationToken ct);
    Task UpdateAsync(Guid id, string name, string email, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);

    Task UpdateLogoAsync(Guid clientId, byte[] logo, string contentType, string fileName, CancellationToken ct);
}