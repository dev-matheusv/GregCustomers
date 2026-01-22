using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClient;

public class UpdateClientCommandHandler
    : IRequestHandler<UpdateClientCommand>
{
    private readonly IClientCommands _commands;

    public UpdateClientCommandHandler(IClientCommands commands)
        => _commands = commands;

    public async Task Handle(UpdateClientCommand request, CancellationToken ct)
    {
        await _commands.UpdateAsync(
            request.Id,
            request.Name,
            request.Email,
            ct
        );
    }
}