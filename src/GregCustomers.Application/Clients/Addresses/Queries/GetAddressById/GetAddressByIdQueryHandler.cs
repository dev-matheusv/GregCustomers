using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Queries.GetAddressById;

public class GetAddressByIdQueryHandler(IAddressQueries addressQueries)
    : IRequestHandler<GetAddressByIdQuery, Address?>
{
    public async Task<Address?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        => await addressQueries.GetByIdAsync(request.Id,  cancellationToken);
}
