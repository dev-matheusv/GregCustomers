using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(IAddressCommands addressCommands)
    : IRequestHandler<UpdateAddressCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken ct)
    {
        await addressCommands.UpdateAsync(request.Id, request.Street, ct);
        return Unit.Value;
    }

}
