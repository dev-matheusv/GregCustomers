using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.DeleteAddress;

public class DeleteAddressCommandHandler(IAddressCommands addressCommands)
    : IRequestHandler<DeleteAddressCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        await addressCommands.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
