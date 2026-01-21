using GregCustomers.Domain.Entities;

namespace GregCustomers.Application.Abstractions.Persistence;

public interface IClientQueries
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Client>> GetAllAsync(int page, int pageSize, CancellationToken ct);
}