using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Queries.GetAddressById;

public record GetAddressByIdQuery(Guid Id) : IRequest<Address?>;
