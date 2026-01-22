using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Application.Clients.DTOs;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClientById;

public class GetClientByIdQueryHandler(IClientQueries queries) : IRequestHandler<GetClientByIdQuery, ClientResponse?>
{
    public async Task<ClientResponse?> Handle(GetClientByIdQuery request, CancellationToken ct)
    {
        var client = await queries.GetByIdAsync(request.Id, ct);

        if (client is null)
            return null;

        return new ClientResponse(
            client.Id,
            client.Name,
            client.Email,
            client.Addresses
                .Select(a => new AddressResponse(a.Id, a.Street))
                .ToList()
        );
    }
}