using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(
    Guid ClientId,
    string Street
) : IRequest<Guid>;
