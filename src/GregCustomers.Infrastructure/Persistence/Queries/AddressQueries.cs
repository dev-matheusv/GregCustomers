using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GregCustomers.Infrastructure.Persistence.Queries;

public class AddressQueries(GregCustomersDbContext db) : IAddressQueries
{
    public Task<Address?> GetByIdAsync(Guid id, CancellationToken ct)
        => db.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<Address>> GetAllAddressesByClientId(Guid clientId, CancellationToken ct)
        => await db.Addresses.AsNoTracking()
            .Where(a => a.ClientId == clientId)
            .OrderBy(a => a.Street)
            .ToListAsync(ct);
}