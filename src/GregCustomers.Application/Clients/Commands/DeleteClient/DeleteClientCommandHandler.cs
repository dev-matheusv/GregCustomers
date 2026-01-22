using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.DeleteClient;

public class DeleteClientCommandHandler
    : IRequestHandler<DeleteClientCommand>
{
    private readonly IClientCommands _commands;

    public DeleteClientCommandHandler(IClientCommands commands)
        => _commands = commands;

    public async Task Handle(DeleteClientCommand request, CancellationToken ct)
    {
        await _commands.DeleteAsync(request.Id, ct);
    }
}