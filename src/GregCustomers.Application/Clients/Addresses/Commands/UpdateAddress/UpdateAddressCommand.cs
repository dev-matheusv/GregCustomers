using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    Guid Id,
    string Street
) : IRequest<Unit>;
