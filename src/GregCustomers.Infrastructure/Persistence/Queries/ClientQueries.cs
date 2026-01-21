using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GregCustomers.Infrastructure.Persistence.Queries;

public class ClientQueries : IClientQueries
{
    private readonly GregCustomersDbContext _db;

    public ClientQueries(GregCustomersDbContext db) => _db = db;

    public Task<Client?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Clients
            .AsNoTracking()
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Client>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        return await _db.Clients
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}