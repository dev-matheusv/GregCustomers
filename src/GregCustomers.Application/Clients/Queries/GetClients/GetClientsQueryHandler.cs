using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Application.Clients.DTOs;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClients;

public class GetClientsQueryHandler
    : IRequestHandler<GetClientsQuery, IReadOnlyList<ClientResponse>>
{
    private readonly IClientQueries _queries;

    public GetClientsQueryHandler(IClientQueries queries)
        => _queries = queries;

    public async Task<IReadOnlyList<ClientResponse>> Handle(GetClientsQuery request, CancellationToken ct)
    {
        var clients = await _queries.GetAllAsync(request.Page, request.PageSize, ct);

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