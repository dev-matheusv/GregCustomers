using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.DeleteClient;

public class DeleteClientCommandHandler(IClientCommands commands, IClientQueries queries)
    : IRequestHandler<DeleteClientCommand>
{
    public async Task Handle(DeleteClientCommand request, CancellationToken ct)
    {
        var exists = await queries.GetByIdAsync(request.Id, ct);
        if (exists is null)
            throw new KeyNotFoundException("Client not found.");

        await commands.DeleteAsync(request.Id, ct);
    }
}
