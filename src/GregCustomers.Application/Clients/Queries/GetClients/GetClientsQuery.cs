using GregCustomers.Application.Clients.DTOs;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClients;

public record GetClientsQuery(int Page, int PageSize)
    : IRequest<IReadOnlyList<ClientResponse>>;