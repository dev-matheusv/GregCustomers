using Dapper;
using GregCustomers.Application.Abstractions.Persistence;

namespace GregCustomers.Infrastructure.Persistence.Commands;

public class ClientCommands : IClientCommands
{
    private readonly ISqlConnectionFactory _factory;

    public ClientCommands(ISqlConnectionFactory factory) => _factory = factory;

    public async Task CreateAsync(Guid id, string name, string email, CancellationToken ct)
    {
        using var conn = _factory.Create();
        var p = new { Id = id, Name = name, Email = email };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_client_create", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task UpdateAsync(Guid id, string name, string email, CancellationToken ct)
    {
        using var conn = _factory.Create();
        var p = new { Id = id, Name = name, Email = email };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_client_update", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        using var conn = _factory.Create();
        var p = new { Id = id };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_client_delete", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task UpdateLogoAsync(Guid clientId, byte[] logo, string contentType, string fileName, CancellationToken ct)
    {
        using var conn = _factory.Create();
        var p = new { ClientId = clientId, Logo = logo, LogoContentType = contentType, LogoFileName = fileName };
        await conn.ExecuteAsync(new CommandDefinition("dbo.sp_client_update_logo", p, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));
    }
}