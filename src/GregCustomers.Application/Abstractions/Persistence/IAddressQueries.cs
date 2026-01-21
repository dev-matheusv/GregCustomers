using GregCustomers.Domain.Entities;

namespace GregCustomers.Application.Abstractions.Persistence;

public interface IAddressQueries
{
    Task<Address?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Address>> GetByClientIdAsync(Guid clientId, CancellationToken ct);
}