using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClientLogo;

public class UpdateClientLogoCommandHandler(IClientCommands commands, IClientQueries queries)
    : IRequestHandler<UpdateClientLogoCommand>
{
    public async Task Handle(UpdateClientLogoCommand request, CancellationToken ct)
    {
        var exists = await queries.GetByIdAsync(request.ClientId, ct);
        if (exists is null)
            throw new KeyNotFoundException("Client not found.");

        await commands.UpdateLogoAsync(request.ClientId, request.Logo, request.ContentType, request.FileName, ct);
    }
}
