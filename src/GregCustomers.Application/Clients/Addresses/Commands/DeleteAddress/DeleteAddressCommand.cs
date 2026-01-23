using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(Guid Id) : IRequest<Unit>;
