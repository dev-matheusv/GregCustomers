using MediatR;

namespace GregCustomers.Application.Clients.Commands.CreateClient;

public record CreateClientCommand(
    string Name,
    string Email
) : IRequest<Guid>;