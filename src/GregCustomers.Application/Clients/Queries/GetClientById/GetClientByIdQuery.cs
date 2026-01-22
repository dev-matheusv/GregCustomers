using GregCustomers.Application.Clients.DTOs;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClientById;

public record GetClientByIdQuery(Guid Id)
    : IRequest<ClientResponse?>;