using GregCustomers.Application.Abstractions.Persistence;
using GregCustomers.Domain.Entities;
using MediatR;

namespace GregCustomers.Application.Clients.Commands.CreateClient;

public class CreateClientCommandHandler
    : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly IClientCommands _commands;

    public CreateClientCommandHandler(IClientCommands commands)
        => _commands = commands;

    public async Task<Guid> Handle(CreateClientCommand request, CancellationToken ct)
    {
        var client = new Client(request.Name, request.Email);

        await _commands.CreateAsync(
            client.Id,
            client.Name,
            client.Email,
            ct
        );

        return client.Id;
    }
}