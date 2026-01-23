using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Queries.GetAddressesByClientId;

public class GetAddressesByClientIdQueryHandler(IAddressQueries addressQueries)
    : IRequestHandler<GetAddressesByClientIdQuery, IEnumerable<Address>>
{
    public async Task<IEnumerable<Address>> Handle(
        GetAddressesByClientIdQuery request,
        CancellationToken cancellationToken
    ) => await addressQueries.GetAllAddressesByClientId(request.ClientId, cancellationToken);
}
