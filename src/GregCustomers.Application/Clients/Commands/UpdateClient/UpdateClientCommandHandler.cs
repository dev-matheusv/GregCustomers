using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClient;

public class UpdateClientCommandHandler(IClientCommands commands, IClientQueries queries)
    : IRequestHandler<UpdateClientCommand>
{
    public async Task Handle(UpdateClientCommand request, CancellationToken ct)
    {
        var exists = await queries.GetByIdAsync(request.Id, ct);
        if (exists is null)
            throw new KeyNotFoundException("Client not found.");

        await commands.UpdateAsync(request.Id, request.Name, request.Email, ct);
    }
}