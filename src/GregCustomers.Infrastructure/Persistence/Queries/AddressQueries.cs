using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GregCustomers.Infrastructure.Persistence.Queries;

public class AddressQueries : IAddressQueries
{
    private readonly GregCustomersDbContext _db;

    public AddressQueries(GregCustomersDbContext db) => _db = db;

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<Address>> GetByClientIdAsync(Guid clientId, CancellationToken ct)
        => await _db.Addresses.AsNoTracking()
            .Where(a => a.ClientId == clientId)
            .OrderBy(a => a.Street)
            .ToListAsync(ct);
}