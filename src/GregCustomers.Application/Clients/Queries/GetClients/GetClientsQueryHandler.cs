using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Application.Clients.DTOs;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClients;

public class GetClientsQueryHandler(IClientQueries queries)
    : IRequestHandler<GetClientsQuery, IReadOnlyList<ClientResponse>>
{
    public async Task<IReadOnlyList<ClientResponse>> Handle(GetClientsQuery request, CancellationToken ct)
    {
        var clients = await queries.GetAllAsync(request.Page, request.PageSize, ct);

        return clients
            .Select(c => new ClientResponse(
                c.Id,
                c.Name,
                c.Email,
                Array.Empty<AddressResponse>() // lista simples
            ))
            .ToList();
    }
}