using Dapper;
using GregCustomers.Application.Abstractions.Persistence;

namespace GregCustomers.Infrastructure.Persistence.Commands;

public class AddressCommands(ISqlConnectionFactory factory) : IAddressCommands
{
    public async Task CreateAsync(Guid id, Guid clientId, string street, CancellationToken ct)
    {
        using var conn = factory.Create();
        var p = new { Id = id, ClientId = clientId, Street = street };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_address_create", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task UpdateAsync(Guid id, string street, CancellationToken ct)
    {
        using var conn = factory.Create();
        var p = new { Id = id, Street = street };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_address_update", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        using var conn = factory.Create();
        var p = new { Id = id };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_address_delete", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }
}