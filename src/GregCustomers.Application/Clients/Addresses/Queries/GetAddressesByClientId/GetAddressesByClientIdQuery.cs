using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Queries.GetAddressesByClientId;

public record GetAddressesByClientIdQuery(Guid ClientId)
    : IRequest<IEnumerable<Address>>;
