using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Infrastructure.Persistence;
using GregCustomers.Infrastructure.Persistence.Commands;
using GregCustomers.Infrastructure.Persistence.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GregCustomers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        services.AddDbContext<GregCustomersDbContext>(opt =>
            opt.UseSqlServer(cs));

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(cs));

        services.AddScoped<IClientQueries, ClientQueries>();
        services.AddScoped<IAddressQueries, AddressQueries>();

        services.AddScoped<IClientCommands, ClientCommands>();
        services.AddScoped<IAddressCommands, AddressCommands>();

        return services;
    }
}