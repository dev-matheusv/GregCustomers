using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClientLogo;

public class UpdateClientLogoCommandHandler : IRequestHandler<UpdateClientLogoCommand>
{
    private readonly IClientCommands _commands;

    public UpdateClientLogoCommandHandler(IClientCommands commands)
        => _commands = commands;

    public async Task Handle(UpdateClientLogoCommand request, CancellationToken ct)
    {
        await _commands.UpdateLogoAsync(
            request.ClientId,
            request.Logo,
            request.ContentType,
            request.FileName,
            ct
        );
    }
}