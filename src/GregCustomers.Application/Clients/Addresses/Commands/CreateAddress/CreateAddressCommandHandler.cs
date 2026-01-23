//using GregCustomers.Application.Abstractions.Persistence;

using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(IAddressCommands addressCommands) : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken ct)
    {
        var id = Guid.NewGuid();

        await addressCommands.CreateAsync(
            id,
            request.ClientId,
            request.Street,
            ct
        );

        return id;
    }
}